namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApplicationUpdate;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using NLog;

    public class LegacyGetCandidateApplicationsStrategy : IGetCandidateApplicationsStrategy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApplicationStatusUpdater _applicationStatusUpdater;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IVacancyStatusProvider _vacancyStatusProvider;

        public LegacyGetCandidateApplicationsStrategy(ICandidateReadRepository candidateReadRepository,
            ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApplicationStatusUpdater applicationStatusUpdater, IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository,
            IVacancyStatusProvider vacancyStatusProvider)
        {
            _candidateReadRepository = candidateReadRepository;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationStatusUpdater = applicationStatusUpdater;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _vacancyStatusProvider = vacancyStatusProvider;
        }

        public IList<ApplicationSummary> GetApplications(Guid candidateId)
        {
            // try to get the latest status of apps for the specified candidate from legacy
            try
            {
                var candidate = _candidateReadRepository.Get(candidateId);

                // (1) call ILegacyApplicationsProvider.GetCandidateApplicationStatuses
                var applicationStatuses = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(candidate).ToList();

                // (2) update the candidate's application with statuses from (1)
                _applicationStatusUpdater.Update(candidate, applicationStatuses);

                // (3) for any draft applications, update status based on current vacancy status
                //var candidateApplications = _applicationReadRepository.GetForCandidate(candidateId);
                //var candidateDraftApplicationIds = candidateApplications
                //    .Where(a => a.Status == ApplicationStatuses.Draft)
                //    .Select(a => a.ApplicationId);

                //foreach (var applicationId in candidateDraftApplicationIds) //todo: parallel?
                //{
                //    var application = _applicationReadRepository.Get(applicationId);
                //    var vacancyId = application.LegacyApplicationId;
                //    var vacancyStatus = _vacancyStatusProvider.GetVacancyStatus(vacancyId);

                //    //todo: update each application status for any that are NOT VacancyStatuses.Live
                //    if (blah)
                //    {
                //        application.Status = ApplicationStatuses.Expired;
                //        _applicationWriteRepository.Save(application);
                //    }
                //}
            }
            catch (Exception ex)
            {
                // if fails just return apps in their current status
                Logger.Warn("Failed to update candidate's application statuses from legacy", ex);
            }

            return _applicationReadRepository.GetForCandidate(candidateId);
        }
    }
}
