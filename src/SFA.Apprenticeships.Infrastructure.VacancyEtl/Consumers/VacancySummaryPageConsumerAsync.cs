namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using SFA.Apprenticeships.Application.VacancyEtl;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;

    public class VacancySummaryPageConsumerAsync : IConsumeAsync<VacancySummaryPage>
    {
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public VacancySummaryPageConsumerAsync(IVacancySummaryProcessor vacancySummaryProcessor)
        {
            _vacancySummaryProcessor = vacancySummaryProcessor;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryPageConsumerAsync")]
        public Task Consume(VacancySummaryPage vacancySummaryPage)
        {
            return Task.Run(() => ConsumeTask(vacancySummaryPage));
        }

        private void ConsumeTask(VacancySummaryPage vacancySummaryPage)
        {
            _vacancySummaryProcessor.QueueVacancySummaries(vacancySummaryPage);
        }
    }
}
