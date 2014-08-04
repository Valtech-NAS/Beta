namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using ApplicationUpdate;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class LegacyGetCandidateApplicationsStrategy : IGetCandidateApplicationsStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApplicationStatusUpdater _applicationStatusUpdater;
        private readonly IApplicationReadRepository _applicationReadRepository;

        public LegacyGetCandidateApplicationsStrategy(ICandidateReadRepository candidateReadRepository, ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider, IApplicationStatusUpdater applicationStatusUpdater, IApplicationReadRepository applicationReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationStatusUpdater = applicationStatusUpdater;
            _applicationReadRepository = applicationReadRepository;
        }

        public IList<ApplicationSummary> GetApplications(Guid candidateId)
        {
            // need to get latest status of apps for the specified candidate from legacy (assuming low latency so can do this synchronously)
            var candidate = _candidateReadRepository.Get(candidateId); 

            // (1) call ILegacyApplicationsProvider.GetCandidateApplicationStatuses
            var applicationStatuses = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(candidate);

            // (2) update the candidate's application with statuses from (1)
            _applicationStatusUpdater.Update(candidate, applicationStatuses);

            // (3) read updated app from Apps repo
            return _applicationReadRepository.GetForCandidate(candidateId);
        }
    }
}
