namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Domain.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using NLog;

    public class VacancySummaryPageConsumerAsync : IConsumeAsync<VacancySummaryPage>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMessageBus _messageBus;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public VacancySummaryPageConsumerAsync(IMessageBus messageBus, IVacancySummaryProcessor vacancySummaryProcessor)
        {
            _messageBus = messageBus;
            _vacancySummaryProcessor = vacancySummaryProcessor;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryPageConsumerAsync")]
        public Task Consume(VacancySummaryPage vacancySummaryPage)
        {
            return Task.Run(() => ConsumeTask(vacancySummaryPage));
        }

        private void ConsumeTask(VacancySummaryPage vacancySummaryPage)
        {
            _vacancySummaryProcessor.QueueVacancySummaries(vacancySummaryPage);

            if (vacancySummaryPage.PageNumber == vacancySummaryPage.TotalPages)
            {
                Logger.Info("Vacancy ETL Queue completed: {0} vacancy summary pages queued ", vacancySummaryPage.TotalPages);

                Logger.Info("Publishing VacancySummaryUpdateComplete message to queue");
                
                var vsuc = new VacancySummaryUpdateComplete
                {
                    ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime
                };

                _messageBus.PublishMessage(vsuc);

                Logger.Info("Published VacancySummaryUpdateComplete message published to queue");
            }
        }
    }
}
