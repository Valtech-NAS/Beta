namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Threading.Tasks;

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
