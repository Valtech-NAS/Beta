namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using NLog;
    using Strategies;

    public class ApplicationStatusProcessor : IApplicationStatusProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;
        private readonly IMessageBus _messageBus;
        private readonly IProcessControlQueue<StorageQueueMessage> _processControlQueue;

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApplicationReadRepository applicationReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy, 
            IProcessControlQueue<StorageQueueMessage> processControlQueue, 
            IMessageBus messageBus)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationReadRepository = applicationReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
            _processControlQueue = processControlQueue;
            _messageBus = messageBus;
        }

        public void QueueApplicationStatuses()
        {
            // retrieve all status updates from legacy... then queue each one for subsequent processing
            var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetAllApplicationStatuses().ToList();

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));

            Logger.Debug("Pushed {0} application summary status update messages to queue", applicationStatusSummaries.Count());
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            // for a single application, check if the update strategy needs to be invoked
            var application = _applicationReadRepository.Get(a => a.LegacyApplicationId == applicationStatusSummary.LegacyApplicationId);

            if (application == null)
            {
                Logger.Warn("Unable to find/update application with legacy ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return;
            }

            if (applicationStatusSummary.ApplicationStatus != application.Status)
            {
                _applicationStatusUpdateStrategy.Update(application, applicationStatusSummary);
            }
        }
    }
}
