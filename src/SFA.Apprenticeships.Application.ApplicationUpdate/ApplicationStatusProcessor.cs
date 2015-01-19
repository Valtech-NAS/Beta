namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Linq;
    using System.Threading.Tasks;
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
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;
        private readonly IMessageBus _messageBus;

        public ApplicationStatusProcessor(ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy, 
            IMessageBus messageBus)
        {
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
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
                applicationStatusSummary => _messageBus.PublishMessage(applicationStatusSummary));

            Logger.Debug("Queued {0} application status updates for page {1} of {2}", applicationStatusSummaries.Count(), applicationStatusSummaryPage.PageNumber, applicationStatusSummaryPage.TotalPages);
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary)
        {
            Logger.Debug("Processing application summary status update for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
            
            if (!ProcessApprenticeshipsApplication(applicationStatusSummary) && !ProcessTraineeshipsApplication(applicationStatusSummary))
            {
                Logger.Warn("Unable to find/update apprenticeship of traineeship application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
            }
        }

        private bool ProcessApprenticeshipsApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (apprenticeshipApplicationDetail == null)
            {
                Logger.Debug("Unable to find/update apprenticeship application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return false;
            }
            return _applicationStatusUpdateStrategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);
        }

        private bool ProcessTraineeshipsApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (traineeshipApplicationDetail == null)
            {
                Logger.Debug("Unable to find/update traineeship application status for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);
                return false;
            }

            return _applicationStatusUpdateStrategy.Update(traineeshipApplicationDetail, applicationStatusSummary);
        }
    }
}
