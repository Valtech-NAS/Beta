using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;

namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Consumers
{
    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummary>
    {
        public VacancySummaryConsumerAsync(IElasticSearchService service)
        {
            
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerAsync")]
        public Task Consume(VacancySummary message)
        {
            return Task.Run(() => ConsumeTask(message));
        }

        private void ConsumeTask(VacancySummary message)
        {
           // Console.WriteLine("TestMessageConsumerAsync recieved message with TestString:" + message.TestString);
        }
    }
}
