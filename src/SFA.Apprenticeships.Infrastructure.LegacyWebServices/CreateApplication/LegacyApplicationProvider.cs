namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using Wcf;

    using ApplicationErrorCodes = Application.Interfaces.Applications.ErrorCodes;
    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;
    using VacancyErrorCodes = Application.Interfaces.Vacancies.ErrorCodes;

    public class LegacyApplicationProvider : ILegacyApplicationProvider
    {
        private readonly ILogService _logger;
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
            ICandidateReadRepository candidateReadRepository,
            ILogService logger)
        {
            _service = service;
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        public int CreateApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            var legacyCandidateId = GetLegacyCandidateId(apprenticeshipApplicationDetail.CandidateId);
            var request = CreateRequest(apprenticeshipApplicationDetail, legacyCandidateId);

            return InternalCreateApplication(apprenticeshipApplicationDetail.CandidateId, request);
        }

        public int CreateApplication(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            var legacyCandidateId = GetLegacyCandidateId(traineeshipApplicationDetail.CandidateId);
            var request = CreateRequest(traineeshipApplicationDetail, legacyCandidateId);

            return InternalCreateApplication(traineeshipApplicationDetail.CandidateId, request);
        }

        #region Helpers

        private int InternalCreateApplication(Guid candidateId, CreateApplicationRequest request)
        {
            try
            {
                _logger.Debug(
                    "Calling Legacy.CreateApplication for candidate id='{0}' and apprenticeship vacancy id='{1}'",
                    candidateId, request.Application.VacancyId);

                var legacyApplicationId = SendRequest(candidateId, request);

                _logger.Debug(
                    "Legacy.CreateApplication succeeded for candidate id='{0}', apprenticeship vacancy id='{1}', legacy application id='{2}'",
                    candidateId, request.Application.VacancyId,
                    legacyApplicationId);

                return legacyApplicationId;
            }
            catch (DomainException e)
            {
                if (e.Code == ApplicationErrorCodes.ApplicationCreationFailed)
                    _logger.Error(e);
                else
                    _logger.Warn(e);

                throw;
            }
            catch (BoundaryException e)
            {
                var de = new DomainException(ApplicationErrorCodes.ApplicationCreationFailed, e, new { candidateId, request.Application.VacancyId });

                _logger.Error(de);
                throw de;
            }
            catch (Exception e)
            {
                _logger.Error(e, new { candidateId, request.Application.VacancyId });
                throw;
            }
        }

        private int SendRequest(Guid candidateId, CreateApplicationRequest request)
        {
            CreateApplicationResponse response = null;

            _service.Use("SecureService", client => response = client.CreateApplication(request));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                string message;
                string errorCode;

                if (response == null)
                {
                    message = "No response";
                    errorCode = ApplicationErrorCodes.ApplicationCreationFailed;
                }
                else if (IsDuplicateError(response))
                {
                    message = "Duplicate application";
                    errorCode = ApplicationErrorCodes.ApplicationDuplicatedError;
                }
                else
                {
                    ParseValidationError(response, out message, out errorCode);
                }

                throw new DomainException(errorCode, new { message, candidateId, request.Application.VacancyId });
            }

            return response.ApplicationId;
        }


        private static bool IsDuplicateError(CreateApplicationResponse response)
        {
            return response.ValidationErrors.Any(e => e.ErrorCode == ValidationErrorCodes.DuplicateApplication);
        }

        private static void ParseValidationError(CreateApplicationResponse response, out string message, out string errorCode)
        {
            var map = new Dictionary<string, string>
            {
                { ValidationErrorCodes.InvalidCandidateState, CandidateErrorCodes.CandidateStateError },
                { ValidationErrorCodes.CandidateNotFound, CandidateErrorCodes.CandidateNotFoundError },
                { ValidationErrorCodes.UnknownCandidate, CandidateErrorCodes.CandidateNotFoundError },
                { ValidationErrorCodes.InvalidVacancyState, VacancyErrorCodes.LegacyVacancyStateError }
            };

            foreach (var pair in map)
            {
                var validationError = response.ValidationErrors.FirstOrDefault(each => each.ErrorCode == pair.Key);

                if (validationError != null)
                {
                    message = string.Format("{0} (ErrorCode='{1}')", validationError.Message, pair.Key);
                    errorCode = pair.Value;
                    return;
                }
            }

            // Failed to parse expected validation error.
            message = string.Format("{0} unexpected validation error(s): {1}",
                response.ValidationErrors.Count(), JsonConvert.SerializeObject(response, Formatting.None));

            errorCode = ApplicationErrorCodes.ApplicationCreationFailed;
        }

        private static CreateApplicationRequest CreateRequest(
            ApprenticeshipApplicationDetail apprenticeshipApplicationDetail, int legacyCandidateId)
        {
            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = apprenticeshipApplicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = legacyCandidateId,
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

        private static CreateApplicationRequest CreateRequest(
            TraineeshipApplicationDetail traineeshipApplicationDetail, int legacyCandidateId)
        {
            return new CreateApplicationRequest
            {
                Application = new Application
                {
                    VacancyId = traineeshipApplicationDetail.Vacancy.Id,
                    VacancyRef = null, // not required if VacancyId is supplied.
                    CandidateId = legacyCandidateId,
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

        private int GetLegacyCandidateId(Guid candidateId)
        {
            return _candidateReadRepository.Get(candidateId, true).LegacyCandidateId;
        }

        #endregion
    }
}
