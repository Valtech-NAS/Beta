namespace SFA.Apprenticeships.Services.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
    using SFA.Apprenticeships.Services.VacancyEtl.Load;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummary>
    {
        private readonly ElasticsearchLoad<VacancySummary> _loader;
 
        public VacancySummaryConsumerAsync(IElasticsearchService service)
        {
            _loader = new ElasticsearchLoad<VacancySummary>(service);
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
                _loader.Execute(message);
            }
            catch (Exception ex)
            {
                // TODO::High::Log this error
            }
        }
    }
}
