namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidate.Strategies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using NLog;
    using Wcf;

    public class LegacyApplicationProvider : ILegacyApplicationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly ICandidateReadRepository _candidateReadRepository;

        private static class ValidationErrorCodes
        {
            public const string InvalidCandidateState = "INVALID_CANDIDATE_STATE";
            public const string DuplicateApplication = "DUPLICATE_APPLICATION";
            public const string CandidateNotFound = "CANDIDATE_NOT_FOUND";
            public const string UnknownCandidate = "UNKNOWN_CANDIDATE";
            public const string InvalidVacancyState = "INVALID_VACANCY_STATE";
        }

        public LegacyApplicationProvider(
            IWcfService<GatewayServiceContract> service,
            ICandidateReadRepository candidateReadRepository)
        {
            _service = service;
            _candidateReadRepository = candidateReadRepository;
        }

        public int CreateApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            var candidate = _candidateReadRepository.Get(apprenticeshipApplicationDetail.CandidateId);
            var legacyRequest = MapApplicationToLegacyRequest(apprenticeshipApplicationDetail, candidate);

            CreateApplicationResponse response = null;

            Logger.Debug("Calling Legacy.CreateApplication for candidate '{0}' and vacancy '{1}'",
                apprenticeshipApplicationDetail.CandidateId,
                apprenticeshipApplicationDetail.Vacancy.Id);

            _service.Use("SecureService", client => response = client.CreateApplication(legacyRequest));

            if (response != null && (response.ValidationErrors == null || !response.ValidationErrors.Any()))
            {
                return response.ApplicationId;
            }

            if (response != null)
            {
                if (response.ValidationErrors.Any(e => e.ErrorCode == ValidationErrorCodes.DuplicateApplication))
                {
                    var warnMessage = string.Format("Duplicate application for candidate '{0}' and vacancy '{1}'",
                        apprenticeshipApplicationDetail.Vacancy.Id, 
                        apprenticeshipApplicationDetail.EntityId);

                    Logger.Warn(warnMessage);

                    throw new CustomException(warnMessage, ErrorCodes.ApplicationDuplicatedError);
                }

                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.InvalidCandidateState, ErrorCodes.LegacyCandidateStateError);

                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.CandidateNotFound, ErrorCodes.LegacyCandidateNotFoundError);
                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.UnknownCandidate, ErrorCodes.LegacyCandidateNotFoundError);

                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.InvalidVacancyState, ErrorCodes.LegacyVacancyStateError);

                var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                Logger.Error("Legacy CreateApplication reported {0} validation error(s): {1}",
                    response.ValidationErrors.Count(),
                    responseAsJson);
            }
            else
            {
                Logger.Error("Legacy.CreateApplication did not respond");
            }

            throw new CustomException(
                string.Format("Failed to create apprenticeship application for candidate '{0}' and vacancy '{1}' in Legacy.CreateApplication",
                    apprenticeshipApplicationDetail.CandidateId, 
                    apprenticeshipApplicationDetail.Vacancy.Id), 
                    ErrorCodes.ApplicationGatewayCreationError);
        }

        private static void CheckValidationErrors(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail, CreateApplicationResponse response, string validationErrorCode, string errorCode)
        {
            if (response.ValidationErrors.Any(e => e.ErrorCode == validationErrorCode))
            {
                var validationError = response.ValidationErrors
                    .First(e => e.ErrorCode == validationErrorCode);

                var warnMessage = string.Format("Unable to create apprenticeship application {0} for candidate {1} in legacy system: \"{2}\".",
                    apprenticeshipApplicationDetail.Vacancy.Id, apprenticeshipApplicationDetail.CandidateId, validationError.Message);

                Logger.Warn(warnMessage);

                throw new CustomException(warnMessage, errorCode);
            }
        }

        private static CreateApplicationRequest MapApplicationToLegacyRequest(
            ApprenticeshipApplicationDetail apprenticeshipApplicationDetail,
            Domain.Entities.Candidates.Candidate candidate)
        {
            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = apprenticeshipApplicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = candidate.LegacyCandidateId,
                    School = MapSchool(apprenticeshipApplicationDetail),
                    EducationResults = MapQualifications(apprenticeshipApplicationDetail.CandidateInformation.Qualifications),
                    WorkExperiences = MapWorkExperience(apprenticeshipApplicationDetail.CandidateInformation.WorkExperience),
                    AdditionalQuestion1Answer = apprenticeshipApplicationDetail.AdditionalQuestion1Answer ?? string.Empty,
                    AdditionalQuestion2Answer = apprenticeshipApplicationDetail.AdditionalQuestion2Answer ?? string.Empty,
                    Strengths = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.Strengths ?? String.Empty,
                    Improvements = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.Improvements ?? String.Empty,
                    HobbiesAndInterests = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.HobbiesAndInterests ?? String.Empty,
                    InterviewSupport = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.Support ?? String.Empty
                }
            };
        }

        private static School MapSchool(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            var educationHistory = apprenticeshipApplicationDetail.CandidateInformation.EducationHistory;

            if (educationHistory == null)
            {
                return null;
            }

            return new School
            {
                Name = educationHistory.Institution,
                Town = null,
                AttendedFrom = MapYearToDate(educationHistory.FromYear),
                AttendedTo = MapYearToDate(educationHistory.ToYear)
            };
        }

        private static EducationResult[] MapQualifications(IEnumerable<Qualification> qualifications)
        {
            const int maxLevelLength = 100;

            return qualifications.Select(each => new EducationResult
            {
                Subject = each.Subject,
                DateAchieved = MapYearToDate(each.Year),
                Level = each.QualificationType.Substring(0, Math.Min(each.QualificationType.Length, maxLevelLength)),
                Grade = MapGrade(each.Grade, each.IsPredicted)
            }).ToArray();
        }

        private static string MapGrade(string grade, bool isPredicted)
        {
            return isPredicted ? string.Format("{0}-Pred", grade) : grade;
        }

        private static GatewayServiceProxy.WorkExperience[] MapWorkExperience(
            IEnumerable<Domain.Entities.Candidates.WorkExperience> workExperience)
        {
            const int maxTypeOfWorkLength = 200;

            return workExperience.Select(each => new GatewayServiceProxy.WorkExperience
            {
                Employer = each.Employer,
                FromDate = each.FromDate,
                ToDate = each.ToDate,
                TypeOfWork = each.Description.Substring(0, Math.Min(each.Description.Length, maxTypeOfWorkLength)),
                PartialCompletion = false, // no mapping available.
                Voluntary = false // no mapping available.
            }).ToArray();
        }

        private static DateTime MapYearToDate(int year)
        {
            return new DateTime(year, 1, 1);
        }
    }
}
