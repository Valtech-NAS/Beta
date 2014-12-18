namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Linq;
    using Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using NLog;
    using Vacancy;

    public class SaveCandidateStrategy : ISaveCandidateStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApplicationsStrategy;
        private readonly IVacancyDataProvider<ApprenticeshipVacancyDetail> _apprenticeshipDataProvider;

        public SaveCandidateStrategy(ICandidateWriteRepository candidateWriteRepository,
            IGetCandidateApprenticeshipApplicationsStrategy getCandidateApplicationsStrategy,
            IVacancyDataProvider<ApprenticeshipVacancyDetail> apprenticeshipDataProvider,
            ICandidateReadRepository candidateReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _candidateWriteRepository = candidateWriteRepository;
            _getCandidateApplicationsStrategy = getCandidateApplicationsStrategy;
            _apprenticeshipDataProvider = apprenticeshipDataProvider;
            _candidateReadRepository = candidateReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            var result = _candidateWriteRepository.Save(candidate);

            var candidateApplications = _getCandidateApplicationsStrategy
                .GetApplications(candidate.EntityId)
                .Where(a => a.Status == ApplicationStatuses.Draft)
                .ToList();

            candidateApplications.ForEach(candidateApplication =>
            {
                try
                {
                    var vacancyDetails =
                        _apprenticeshipDataProvider.GetVacancyDetails(candidateApplication.LegacyVacancyId);
                    var reloadedCandidate = _candidateReadRepository.Get(candidate.EntityId);
                    var apprenticeshipApplicationDetail = UpdateApplicationDetail(reloadedCandidate, vacancyDetails);

                    _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);
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

        private ApprenticeshipApplicationDetail UpdateApplicationDetail(Candidate candidate, ApprenticeshipVacancyDetail vacancyDetails)
        {
            var currentApprenticeshipApplicationDetail =
                _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, a => a.Vacancy.Id == vacancyDetails.Id);

            currentApprenticeshipApplicationDetail.CandidateDetails = candidate.RegistrationDetails;

            return currentApprenticeshipApplicationDetail;
        }
    }
}
