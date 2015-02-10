namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using System.Collections.Generic;
    using ApplicationUpdate;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;

    public class LegacyGetCandidateApprenticeshipApplicationsStrategy : IGetCandidateApprenticeshipApplicationsStrategy
    {
        private readonly ILogService _logger;

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApplicationStatusUpdater _applicationStatusUpdater;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public LegacyGetCandidateApprenticeshipApplicationsStrategy(
            ICandidateReadRepository candidateReadRepository,
            ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApplicationStatusUpdater applicationStatusUpdater,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, ILogService logger)
        {
            _candidateReadRepository = candidateReadRepository;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationStatusUpdater = applicationStatusUpdater;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _logger = logger;
        }

        public IList<ApprenticeshipApplicationSummary> GetApplications(Guid candidateId)
        {
            try
            {
                // try to get the latest status of apps for the specified candidate from legacy
                var candidate = _candidateReadRepository.Get(candidateId);

                if (candidate.LegacyCandidateId != 0)
                {
                    //Verify candidate exists in legacy system otherwise this call will throw and exception and log an error
                    var submittedApplicationStatuses = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(candidate);

                    _applicationStatusUpdater.Update(candidate, submittedApplicationStatuses);

                //todo: queue drafts for status updates?                
            }
            catch (Exception ex)
            {
                // if fails just return apps with their current status
                var message = string.Format("Failed to update candidate's application statuses from legacy. CandidateId: {0}", candidateId);
                _logger.Error(message, ex);

            }

            return _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId);
        }
    }
}
