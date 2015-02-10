namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.VacancyEtl.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>
            _apprenticeshipIndexer;

        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipIndexer;
        private readonly ILogService _logger;

        public VacancySummaryCompleteConsumerAsync(
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipIndexer,
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipIndexer, ILogService logger)
        {
            _apprenticeshipIndexer = apprenticeshipIndexer;
            _trainseeshipIndexer = trainseeshipIndexer;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            _logger.Debug("Received vacancy summary update completed message.");

            return Task.Run(() =>
            {
                if (_apprenticeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    _logger.Info("Swapping apprenticeship index alias after vacancy summary update completed");
                    _apprenticeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    _logger.Info("Index apprenticeship swapped after vacancy summary update completed");
                }
                else
                {
                    _logger.Error("The new apprenticeship index is not correctly created. Aborting swap.");
                }

                if (_trainseeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    _logger.Info("Swapping traineeship index alias after vacancy summary update completed");
                    _trainseeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    _logger.Info("Index traineeship swapped after vacancy summary update completed");
                }
                else
                {
                    _logger.Error("The new traineeship index is not correctly created. Aborting swap.");
                }
            });
        }
    }
}
