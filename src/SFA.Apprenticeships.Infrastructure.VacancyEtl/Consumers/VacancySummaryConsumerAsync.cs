using SFA.Apprenticeships.Domain.Interfaces.Mapping;

namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.Interfaces.Search;
    using Application.VacancyEtl.Entities;
    using Elasticsearch.Entities;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummaryUpdate>
    {
        private readonly IMapper _mapper;
        private readonly IIndexingService<VacancySummary> _indexer;

        public VacancySummaryConsumerAsync(IMapper mapper, IIndexingService<VacancySummary> indexer)
        {
            _mapper = mapper;
            _indexer = indexer;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerAsync")]
        public Task Consume(VacancySummaryUpdate message)
        {
            return Task.Run(() => ConsumeTask(message));
        }

        private void ConsumeTask(VacancySummaryUpdate message)
        {
            try
            {
                var messageToIndex = _mapper.Map<VacancySummaryUpdate, VacancySummary>(message);
                _indexer.Index(message.Id.ToString(CultureInfo.InvariantCulture), messageToIndex);
            }
            catch (Exception)
            {
                throw; // TODO::High::Log this error
            }
        }
    }
}
