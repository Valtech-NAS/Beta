namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApplicationUpdate;
    using Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class LegacyGetCandidateApprenticeshipApplicationsStrategy : IGetCandidateApprenticeshipApplicationsStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApplicationStatusUpdater _applicationStatusUpdater;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public LegacyGetCandidateApprenticeshipApplicationsStrategy(
            ICandidateReadRepository candidateReadRepository,
            ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApplicationStatusUpdater applicationStatusUpdater,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationStatusUpdater = applicationStatusUpdater;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public IList<ApprenticeshipApplicationSummary> GetApplications(Guid candidateId)
        {
            // try to get the latest status of apps for the specified candidate from legacy
            try
            {
                var candidate = _candidateReadRepository.Get(candidateId);

                //todo: may optionally skip 1+2 below if recently retrieved... TBC later
                // (1) get the current application statuses for submitted applications from legacy
                var submittedApplicationStatuses = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(candidate).ToList();

                // (2) update the candidate's applications in the repo with statuses from (1)
                _applicationStatusUpdater.Update(candidate, submittedApplicationStatuses);

                //todo: not doing 3 below for now... TBC later
                // (3) for any draft applications, update status based on current vacancy status
                //var candidateApplications = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId);
                //var draftApplicationIds = candidateApplications
                //    .Where(a => a.Status == ApplicationStatuses.Draft)
                //    .Select(a => a.ApplicationId);

                //foreach (var applicationId in draftApplicationIds) //todo: parallel?
                //{
                //    var application = _apprenticeshipApplicationReadRepository.Get(applicationId);
                //    var vacancyStatus = _vacancyStatusProvider.GetVacancyStatus(application.LegacyApplicationId);

                //    //todo: update each application status for any that are NOT VacancyStatuses.Live
                //    //if (blah)
                //    //{
                //    //    application.Status = ApplicationStatuses.Expired;
                //    //    _applicationWriteRepository.Save(application);
                //    //}
                //}
            }
            catch (Exception ex)
            {
                // if fails just return apps with their current status
                Logger.Error("Failed to update candidate's application statuses from legacy", ex);
            }

            return _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId);
        }
    }
}
