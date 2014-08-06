namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private readonly IVacancyIndexerService _vacancyIndexer;

        public VacancySummaryCompleteConsumerAsync(IVacancyIndexerService vacancyIndexer)
        {
            _vacancyIndexer = vacancyIndexer;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            return Task.Run(() => _vacancyIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime));
        }
    }
}
