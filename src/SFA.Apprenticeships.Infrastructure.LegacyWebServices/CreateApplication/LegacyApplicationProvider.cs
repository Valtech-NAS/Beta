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

        public int CreateApplication(ApplicationDetail applicationDetail)
        {
            var message = string.Format("Legacy CreateApplication of candidate {0} to vacancy {1}",
                applicationDetail.CandidateId, applicationDetail.Vacancy.Id);
            Logger.Debug(message);

            var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId);
            var legacyRequest = MapApplicationToLegacyRequest(applicationDetail, candidate);

            CreateApplicationResponse response = null;

            _service.Use("SecureService", client => response = client.CreateApplication(legacyRequest));

            if (response != null && (response.ValidationErrors == null || !response.ValidationErrors.Any()))
            {
                return response.ApplicationId;
            }

            if (response != null)
            {
                if (response.ValidationErrors.Any(e => e.ErrorCode == ValidationErrorCodes.DuplicateApplication))
                {
                    var warnMessage = string.Format("Duplicate application {0} for candidate {1} in legacy system",
                        applicationDetail.Vacancy.Id, applicationDetail.CandidateId);

                    Logger.Warn(warnMessage);

                    throw new CustomException(warnMessage, ErrorCodes.ApplicationDuplicatedError);
                }

                CheckValidationErrors(applicationDetail, response, ValidationErrorCodes.InvalidCandidateState, ErrorCodes.LegacyCandidateStateError);

                CheckValidationErrors(applicationDetail, response, ValidationErrorCodes.CandidateNotFound, ErrorCodes.LegacyCandidateNotFoundError);
                CheckValidationErrors(applicationDetail, response, ValidationErrorCodes.UnknownCandidate, ErrorCodes.LegacyCandidateNotFoundError);

                CheckValidationErrors(applicationDetail, response, ValidationErrorCodes.InvalidVacancyState, ErrorCodes.LegacyVacancyStateError);

                var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                Logger.Error(
                    "Legacy CreateApplication reported {0} validation error(s): {1} when creating application of candidate {2} to vacancy {3}",
                    response.ValidationErrors.Count(), responseAsJson, applicationDetail.CandidateId, applicationDetail.Vacancy.Id);
            }
            else
            {
                Logger.Error("Legacy CreateApplication of candidate {0} to vacancy {1} did not respond.",
                        applicationDetail.CandidateId, applicationDetail.Vacancy.Id);
            }

            throw new CustomException(
                string.Format("Failed to create application of candidate {0} to vacancy {1} in legacy system",
                    applicationDetail.CandidateId, applicationDetail.Vacancy.Id), ErrorCodes.ApplicationGatewayCreationError);
        }

        private static void CheckValidationErrors(ApplicationDetail applicationDetail, CreateApplicationResponse response, string validationErrorCode, string errorCode)
        {
            if (response.ValidationErrors.Any(e => e.ErrorCode == validationErrorCode))
            {
                var validationError = response.ValidationErrors
                    .First(e => e.ErrorCode == validationErrorCode);

                var warnMessage = string.Format("Unable to create application {0} for candidate {1} in legacy system: \"{2}\".",
                    applicationDetail.Vacancy.Id, applicationDetail.CandidateId, validationError.Message);

                Logger.Warn(warnMessage);

                throw new CustomException(warnMessage, errorCode);
            }
        }

        private static CreateApplicationRequest MapApplicationToLegacyRequest(
            ApplicationDetail applicationDetail,
            Domain.Entities.Candidates.Candidate candidate)
        {
            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = applicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = candidate.LegacyCandidateId,
                    School = MapSchool(applicationDetail),
                    EducationResults = MapQualifications(applicationDetail.CandidateInformation.Qualifications),
                    WorkExperiences = MapWorkExperience(applicationDetail.CandidateInformation.WorkExperience),
                    AdditionalQuestion1Answer = applicationDetail.AdditionalQuestion1Answer ?? string.Empty,
                    AdditionalQuestion2Answer = applicationDetail.AdditionalQuestion2Answer ?? string.Empty,
                    Strengths = applicationDetail.CandidateInformation.AboutYou.Strengths ?? String.Empty,
                    Improvements = applicationDetail.CandidateInformation.AboutYou.Improvements ?? String.Empty,
                    HobbiesAndInterests = applicationDetail.CandidateInformation.AboutYou.HobbiesAndInterests ?? String.Empty,
                    InterviewSupport = applicationDetail.CandidateInformation.AboutYou.Support ?? String.Empty
                }
            };
        }

        private static School MapSchool(ApplicationDetail applicationDetail)
        {
            var educationHistory = applicationDetail.CandidateInformation.EducationHistory;

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
