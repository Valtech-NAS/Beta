namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class TraineeshipsSummaryUpdateConsumerAsync : IConsumeAsync<TraineeshipSummaryUpdate>
    {
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _vacancyIndexer;

        public TraineeshipsSummaryUpdateConsumerAsync(IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> vacancyIndexer)
        {
            _vacancyIndexer = vacancyIndexer;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "TraineeshipsSummaryUpdateConsumerAsync")]
        public Task Consume(TraineeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() => _vacancyIndexer.Index(vacancySummaryToIndex));
        }
    }
}
