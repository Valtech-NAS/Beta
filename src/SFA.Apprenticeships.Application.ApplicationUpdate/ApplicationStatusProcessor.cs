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

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApplicationReadRepository applicationReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy, 
            IMessageBus messageBus)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _applicationReadRepository = applicationReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
            _messageBus = messageBus;
        }

        public void QueueApplicationStatuses()
        {
            Logger.Debug("Starting to queue application summary status update messages");

            // retrieve all status updates from legacy... then queue each one for subsequent processing
            var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetAllApplicationStatuses().ToList();

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));

            Logger.Debug("Queued {0} application summary status update messages", applicationStatusSummaries.Count());
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            Logger.Debug("Starting to process application summary status update for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);

            // for a single application, check if the update strategy needs to be invoked
            var application = _applicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (application == null)
            {
                Logger.Warn("Unable to find/update application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return;
            }

            // only if status has changed
            if (applicationStatusSummary.ApplicationStatus != application.Status)
            {
                _applicationStatusUpdateStrategy.Update(application, applicationStatusSummary);
                Logger.Debug("Updated application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
            }
        }
    }
}
