namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummary>
    {
        private readonly IIndexingService<VacancySummary> _indexer;

        public VacancySummaryConsumerAsync(IIndexingService<VacancySummary> indexer)
        {
            _indexer = indexer;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerAsync")]
        public Task Consume(VacancySummary message)
        {
            return Task.Run(() => ConsumeTask(message));
        }

        private void ConsumeTask(VacancySummary message)
        {
            try
            {
                _indexer.Index(message.Id.ToString(CultureInfo.InvariantCulture), message);
            }
            catch (Exception ex)
            {
                // TODO::High::Log this error
            }
        }
    }
}
