namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using NLog;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService _vacancyIndexer;

        public VacancySummaryCompleteConsumerAsync(IVacancyIndexerService vacancyIndexer)
        {
            _vacancyIndexer = vacancyIndexer;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            return Task.Run(() =>
            {
                Logger.Debug("Swapping Index after vacancy summary update completed at {0} ", updateComplete.ScheduledRefreshDateTime);

                _vacancyIndexer.SwapIndex(updateComplete.ScheduledRefreshDateTime);

                Logger.Debug("Index swapped after vacancy summary update completed at {0} ", updateComplete.ScheduledRefreshDateTime);
            });
        }
    }
}
