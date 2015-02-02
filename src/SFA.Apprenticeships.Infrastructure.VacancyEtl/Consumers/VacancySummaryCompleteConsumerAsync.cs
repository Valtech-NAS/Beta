namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.VacancyEtl.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
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
            return Task.Run(() =>
            {
                if (_apprenticeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    _apprenticeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                }

                if (_trainseeshipIndexer.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    _trainseeshipIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                }
            });
        }
    }
}
