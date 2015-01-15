namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidate;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using NLog;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;
    using WorkExperience = GatewayServiceProxy.WorkExperience;

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

            Logger.Debug("Calling Legacy.CreateApplication for candidate '{0}' and apprenticeship vacancy '{1}'",
                apprenticeshipApplicationDetail.CandidateId,
                apprenticeshipApplicationDetail.Vacancy.Id);

            _service.Use("SecureService", client => response = client.CreateApplication(legacyRequest));

            if (response != null && (response.ValidationErrors == null || !response.ValidationErrors.Any()))
            {
                return response.ApplicationId;
            }

            if (response != null)
            {
                CheckDuplicateError(apprenticeshipApplicationDetail, apprenticeshipApplicationDetail.Vacancy.Id, response);
                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.InvalidCandidateState, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateStateError);
                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.CandidateNotFound, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateNotFoundError);
                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.UnknownCandidate, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateNotFoundError);
                CheckValidationErrors(apprenticeshipApplicationDetail, response, ValidationErrorCodes.InvalidVacancyState, Apprenticeships.Application.Interfaces.Vacancies.ErrorCodes.LegacyVacancyStateError);

                var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                Logger.Error("Legacy CreateApplication for apprenticeship reported {0} validation error(s): {1}",
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

        public int CreateApplication(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            var candidate = _candidateReadRepository.Get(traineeshipApplicationDetail.CandidateId);
            var legacyRequest = MapApplicationToLegacyRequest(traineeshipApplicationDetail, candidate);

            CreateApplicationResponse response = null;

            Logger.Debug("Calling Legacy.CreateApplication for candidate '{0}' and traineeship vacancy '{1}'",
                traineeshipApplicationDetail.CandidateId,
                traineeshipApplicationDetail.Vacancy.Id);

            _service.Use("SecureService", client => response = client.CreateApplication(legacyRequest));

            if (response != null && (response.ValidationErrors == null || !response.ValidationErrors.Any()))
            {
                return response.ApplicationId;
            }

            if (response != null)
            {
                CheckDuplicateError(traineeshipApplicationDetail, traineeshipApplicationDetail.Vacancy.Id, response);
                CheckValidationErrors(traineeshipApplicationDetail, response, ValidationErrorCodes.InvalidCandidateState, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateStateError);
                CheckValidationErrors(traineeshipApplicationDetail, response, ValidationErrorCodes.CandidateNotFound, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateNotFoundError);
                CheckValidationErrors(traineeshipApplicationDetail, response, ValidationErrorCodes.UnknownCandidate, Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateNotFoundError);
                CheckValidationErrors(traineeshipApplicationDetail, response, ValidationErrorCodes.InvalidVacancyState, Apprenticeships.Application.Interfaces.Vacancies.ErrorCodes.LegacyVacancyStateError);

                var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                Logger.Error("Legacy CreateApplication for traineeship reported {0} validation error(s): {1}",
                    response.ValidationErrors.Count(),
                    responseAsJson);
            }
            else
            {
                Logger.Error("Legacy.CreateApplication did not respond");
            }

            throw new CustomException(
                string.Format("Failed to create traineeship application for candidate '{0}' and vacancy '{1}' in Legacy.CreateApplication",
                    traineeshipApplicationDetail.CandidateId,
                    traineeshipApplicationDetail.Vacancy.Id),
                    ErrorCodes.ApplicationGatewayCreationError);
        }

        private static void CheckDuplicateError(ApplicationDetail applicationDetail,
            int vacancyId, CreateApplicationResponse response)
        {
            if (response.ValidationErrors.Any(e => e.ErrorCode == ValidationErrorCodes.DuplicateApplication))
            {
                var warnMessage = string.Format("Duplicate application for candidate '{0}' and vacancy '{1}'",
                    applicationDetail.CandidateId, vacancyId);

                Logger.Warn(warnMessage);

                throw new CustomException(warnMessage, Apprenticeships.Application.Interfaces.Applications.ErrorCodes.ApplicationDuplicatedError);
            }
        }

        private static void CheckValidationErrors(ApplicationDetail apprenticeshipApplicationDetail, CreateApplicationResponse response, string validationErrorCode, string errorCode)
        {
            if (response.ValidationErrors.Any(e => e.ErrorCode == validationErrorCode))
            {
                var validationError = response.ValidationErrors
                    .First(e => e.ErrorCode == validationErrorCode);

                var warnMessage = string.Format("Unable to create application {0} for candidate {1} in legacy system: \"{2}\".",
                    apprenticeshipApplicationDetail.EntityId, apprenticeshipApplicationDetail.CandidateId, validationError.Message);

                Logger.Warn(warnMessage);

                throw new CustomException(warnMessage, errorCode);
            }
        }

        private static CreateApplicationRequest MapApplicationToLegacyRequest(
            ApprenticeshipApplicationDetail apprenticeshipApplicationDetail,
            Candidate candidate)
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
                    Strengths = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.Strengths ?? string.Empty,
                    Improvements = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.Improvements ?? string.Empty,
                    HobbiesAndInterests = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.HobbiesAndInterests ?? string.Empty,
                    InterviewSupport = apprenticeshipApplicationDetail.CandidateInformation.AboutYou.Support ?? string.Empty
                }
            };
        }

        private static CreateApplicationRequest MapApplicationToLegacyRequest(
            TraineeshipApplicationDetail traineeshipApplicationDetail,
            Candidate candidate)
        {
            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = traineeshipApplicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = candidate.LegacyCandidateId,
                    School = MapSchool(),
                    EducationResults = MapQualifications(traineeshipApplicationDetail.CandidateInformation.Qualifications),
                    WorkExperiences = MapWorkExperience(traineeshipApplicationDetail.CandidateInformation.WorkExperience),
                    AdditionalQuestion1Answer = traineeshipApplicationDetail.AdditionalQuestion1Answer ?? string.Empty,
                    AdditionalQuestion2Answer = traineeshipApplicationDetail.AdditionalQuestion2Answer ?? string.Empty
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

        private static School MapSchool()
        {
            var fakeAttendanceYear = MapYearToDate(2000);

            return new School
            {
                Name = "N/A",
                AttendedFrom = fakeAttendanceYear,
                AttendedTo = fakeAttendanceYear
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

        private static WorkExperience[] MapWorkExperience(
            IEnumerable<Domain.Entities.Candidates.WorkExperience> workExperience)
        {
            const int maxTypeOfWorkLength = 200;

            return workExperience.Select(each => new WorkExperience
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
