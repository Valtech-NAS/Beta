namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using Vacancy;

    public class SaveCandidateStrategy : ISaveCandidateStrategy
    {
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
                VacancyDetail vacancyDetails =
                    _vacancyDataProvider.GetVacancyDetails(candidateApplication.LegacyVacancyId);
                Candidate reloadedCandidate = _candidateReadRepository.Get(candidate.EntityId);
                ApplicationDetail applicationDetail = UpdateApplicationDetail(reloadedCandidate, vacancyDetails);

                _applicationWriteRepository.Save(applicationDetail);
            });

            return result;
        }

        private ApplicationDetail UpdateApplicationDetail(Candidate candidate, VacancyDetail vacancyDetails)
        {
            ApplicationDetail currentApplicationDetail =
                _applicationReadRepository.GetForCandidate(candidate.EntityId, a => a.Vacancy.Id == vacancyDetails.Id);

            currentApplicationDetail.CandidateDetails = candidate.RegistrationDetails;

            return currentApplicationDetail;
        }
    }
}