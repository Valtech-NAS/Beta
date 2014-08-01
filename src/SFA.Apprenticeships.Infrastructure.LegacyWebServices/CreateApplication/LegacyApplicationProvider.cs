namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidate.Strategies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;

    /*
     * NOTE: legacy qualification types are:
     * 
     *    BTEC First Diploma / First Certificate
     *    NVQ or SVQ Level 1 / GNVQ Foundation
     *    City & Guilds Craft, RSA Diploma or other trade qualifications/apprenticeship at NVQ level 2
     *    CSEs
     *    GCE O Levels
     *    GCSEs (at A*-C)
     *    NVQ or SVQ Level 2 / GNVQ Intermediate / School Certificate / Matriculation
     *    City & Guilds Advanced Craft, RSA Advanced Diploma or other Advanced trade qualification / apprenticeship at NVQ level 3
     *    GCE A Levels
     *    NVQ or SVQ Level 3 / GNVQ Advanced / NNEB 
     *    BTEC National Diploma / National Certificate / OND / ONC
     *    BTEC HNC/HND
     *    NVQ or SVQ Level 4 / RSA Higher Diploma / Foundation Degree
     *    NVQ or SVQ Level 5
     *    Degree (eg BA, Bsc)/Degree Level Nursing or Teaching Qualification
     *    Postgraduate level (eg PG Dip, MA, MSc, Phd)
     *    Professional Qualifications (eg Chartered Accountant)
     *    Non UK Qualifications
     *    Other
     */

    public class LegacyApplicationProvider : ILegacyApplicationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public LegacyApplicationProvider(
            IWcfService<GatewayServiceContract> service,
            ICandidateReadRepository candidateReadRepository)
        {
            _service = service;
            _candidateReadRepository = candidateReadRepository;
        }

        public int CreateApplication(ApplicationDetail applicationDetail)
        {
            var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId);
            var legacyRequest = MapApplicationToLegacyRequest(applicationDetail, candidate);

            CreateApplicationResponse response = null;

            _service.Use(client => response = client.CreateApplication(legacyRequest));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    // TODO: log actual validation errors (same for email).
                    Logger.Error(
                        "Legacy CreateApplication reported {0} validation error(s).",
                        response.ValidationErrors.Count());
                }
                else
                {
                    Logger.Error("Legacy CreateApplication did not respond.");
                }

                // TODO: EXCEPTION: should use an application exception type
                throw new Exception("Failed to create candidate in legacy system");
            }

            return response.ApplicationId;
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
                    AdditionalQuestion2Answer = applicationDetail.AdditionalQuestion2Answer ?? string.Empty
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
                Grade = each.Grade
            }).ToArray();
        }

        private static GatewayServiceProxy.WorkExperience[] MapWorkExperience(
            IEnumerable<Domain.Entities.Candidates.WorkExperience> workExperience)
        {
            const int maxTypeOfWorkLength = 200;

            return workExperience.Select(each => new GatewayServiceProxy.WorkExperience
            {
                Employer = each.Employer,
                FromDate = MapYearToDate(each.FromYear),
                ToDate = MapYearToDate(each.ToYear),
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
