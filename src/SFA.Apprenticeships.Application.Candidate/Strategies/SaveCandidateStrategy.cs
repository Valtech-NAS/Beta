namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using NLog;
    using Vacancy;

    public class SaveCandidateStrategy : ISaveCandidateStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IGetCandidateApplicationsStrategy _getCandidateApplicationsStrategy;
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public SaveCandidateStrategy(ICandidateWriteRepository candidateWriteRepository,
            IGetCandidateApplicationsStrategy getCandidateApplicationsStrategy,
            IVacancyDataProvider vacancyDataProvider,
            ICandidateReadRepository candidateReadRepository,
            IApplicationWriteRepository applicationWriteRepository,
            IApplicationReadRepository applicationReadRepository)
        {
            _candidateWriteRepository = candidateWriteRepository;
            _getCandidateApplicationsStrategy = getCandidateApplicationsStrategy;
            _vacancyDataProvider = vacancyDataProvider;
            _candidateReadRepository = candidateReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _applicationReadRepository = applicationReadRepository;
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            Candidate result = _candidateWriteRepository.Save(candidate);

            List<ApplicationSummary> candidateApplications = _getCandidateApplicationsStrategy
                .GetApplications(candidate.EntityId)
                .Where(a => a.Status == ApplicationStatuses.Draft)
                .ToList();

            candidateApplications.ForEach(candidateApplication =>
            {
                try
                {
                    VacancyDetail vacancyDetails =
                        _vacancyDataProvider.GetVacancyDetails(candidateApplication.LegacyVacancyId);
                    Candidate reloadedCandidate = _candidateReadRepository.Get(candidate.EntityId);
                    ApprenticeshipApplicationDetail apprenticeshipApplicationDetail = UpdateApplicationDetail(reloadedCandidate, vacancyDetails);

                    _applicationWriteRepository.Save(apprenticeshipApplicationDetail);
                }
                catch (Exception e)
                {
                    // Try updating the next one
                    var message =
                        string.Format(
                            "Error while updating an application in draft state with the new user personal details for user {0}",
                            candidate.EntityId);
                    Logger.Error(message, e);
                }
            });

            return result;
        }

        private ApprenticeshipApplicationDetail UpdateApplicationDetail(Candidate candidate, VacancyDetail vacancyDetails)
        {
            ApprenticeshipApplicationDetail currentApprenticeshipApplicationDetail =
                _applicationReadRepository.GetForCandidate(candidate.EntityId, a => a.Vacancy.Id == vacancyDetails.Id);

            currentApprenticeshipApplicationDetail.CandidateDetails = candidate.RegistrationDetails;

            return currentApprenticeshipApplicationDetail;
        }
    }
}