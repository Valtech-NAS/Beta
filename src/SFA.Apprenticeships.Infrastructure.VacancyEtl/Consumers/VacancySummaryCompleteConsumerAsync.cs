namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.VacancyEtl.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using NLog;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>
            _apprenticeshipIndexer;

        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipIndexer;

        public VacancySummaryCompleteConsumerAsync(
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipIndexer,
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipIndexer)
        {
            _apprenticeshipIndexer = apprenticeshipIndexer;
            _trainseeshipIndexer = trainseeshipIndexer;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            Logger.Debug("Received vacancy summary update completed message.");

            return Task.Run(() =>
            {
                if (_apprenticeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    Logger.Info("Swapping apprenticeship index alias after vacancy summary update completed");
                    _apprenticeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    Logger.Info("Index apprenticeship swapped after vacancy summary update completed");
                }
                else
                {
                    Logger.Error("The new apprenticeship index is not correctly created. Aborting swap.");
                }

                if (_trainseeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    Logger.Info("Swapping traineeship index alias after vacancy summary update completed");
                    _trainseeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    Logger.Info("Index traineeship swapped after vacancy summary update completed");
                }
                else
                {
                    Logger.Error("The new traineeship index is not correctly created. Aborting swap.");
                }
            });
        }
    }
}