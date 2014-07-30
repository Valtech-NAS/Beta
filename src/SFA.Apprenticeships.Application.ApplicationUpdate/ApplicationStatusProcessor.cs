namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Entities;
    using NLog;
    using Strategies;

    public class ApplicationStatusProcessor : IApplicationStatusProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider, IApplicationReadRepository applicationReadRepository, IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationReadRepository = applicationReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
        }

        public void QueueApplicationStatuses(StorageQueueMessage scheduledQueueMessage)
        {
            //todo: objective is to create a new queue of all app statuses that need to be processed
            // call the legacy service to retrieve a list of all application status changes ILegacyApplicationStatusesProvider.GetAllApplicationStatuses()
            // note, we'll probably add parameters to the call to legacy to constrain the extract but currently this isn't supported
            // foreach: queue for processing (ApplicationStatusSummary)

            throw new NotImplementedException();
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            //todo: objective is to call the update strategy for the status update if it has changed
            // note, this is NOT the same as IApplicationStatusUpdater.Update() !!!
            // find corresponding application guid, check status, call strategy if changed

            //var application = _applicationReadRepository.Get(a => a.LegacyApplicationId == applicationStatusSummary.LegacyApplicationId);

            //todo: log warning if cannot find
            // log warning as we need to update an application that isn't in the repo
            //Logger.Warn("Unable to find/update application with legacy ID '{0}'", applicationStatusSummary.LegacyApplicationId);

            //_applicationStatusUpdateStrategy.Update(applicationStatusSummary);

            throw new NotImplementedException();
        }
    }
}
