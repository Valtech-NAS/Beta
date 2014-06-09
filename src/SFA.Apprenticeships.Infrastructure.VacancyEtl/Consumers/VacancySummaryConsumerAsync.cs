namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Services;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummaryUpdate>
    {
        private readonly IIndexerService<VacancySummaryUpdate> _indexer;

        public VacancySummaryConsumerAsync(IIndexerService<VacancySummaryUpdate> indexer)
        {
            _indexer = indexer;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerAsync")]
        public Task Consume(VacancySummaryUpdate message)
        {
            return Task.Run(() => _indexer.Index(message));
        }
    }
}
