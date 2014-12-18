namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using NLog;
    using Strategies;

    public class ApplicationStatusProcessor : IApplicationStatusProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;
        private readonly IMessageBus _messageBus;

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy, 
            IMessageBus messageBus)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
            _messageBus = messageBus;
        }

        public void QueueApplicationStatusesPages()
        {
            Logger.Debug("Starting to queue application summary status update pages");

            var pageCount = _legacyApplicationStatusesProvider.GetApplicationStatusesPageCount();

            if (pageCount == 0)
            {
                Logger.Debug("No application status update pages to queue");
                return;
            }

            var pages = Enumerable.Range(1, pageCount)
                .Select(i => new ApplicationUpdatePage {PageNumber = i, TotalPages = pageCount});

            Logger.Debug("Queueing {0} application summary status update pages", pageCount);

            Parallel.ForEach(
                pages,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                page => _messageBus.PublishMessage(page));

            Logger.Debug("Queued {0} application status update pages", pageCount);
        }

        public void QueueApplicationStatuses(ApplicationUpdatePage applicationStatusSummaryPage)
        {
            Logger.Debug("Starting to queue application status updates for page {0} of {1}", applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);

            // retrieve page of status updates from legacy... then queue each one for subsequent processing
            var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetAllApplicationStatuses(applicationStatusSummaryPage.PageNumber).ToList();

            if (!applicationStatusSummaries.Any())
            {
                Logger.Debug("No application status updates to queue");
                return;
            }

            Logger.Debug("Queueing {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary =>
                {
                    var applicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

                    // If the application status has not changed, there's no work to do.
                    //TODO: Should we be doing anything in the event of applicationDetail being null? This happens because we're sharing the nas preprod gateway with four environments
                    if (applicationDetail != null && applicationDetail.Status != applicationStatusSummary.ApplicationStatus)
                    {
                        _messageBus.PublishMessage(applicationStatusSummary);
                    }
                });

            Logger.Debug("Queued {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            Logger.Debug("Processing application summary status update for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);

            // for a single application, check if the update strategy needs to be invoked
            var application = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (application == null)
            {
                Logger.Warn("Unable to find/update application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return;
            }

            // only if status has changed
            if (applicationStatusSummary.ApplicationStatus != application.Status)
            {
                _applicationStatusUpdateStrategy.Update(application, applicationStatusSummary);
                Logger.Debug("Updated application status for application with legacy application ID '{0}'",
                    applicationStatusSummary.LegacyApplicationId);
            }
            else
            {
                Logger.Debug("Skipped application status for application with legacy application ID '{0}'",
                    applicationStatusSummary.LegacyApplicationId);
            }
        }
    }
}
