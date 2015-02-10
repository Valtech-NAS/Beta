namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;

    public class VacancySummaryPageConsumerAsync : IConsumeAsync<VacancySummaryPage>
    {
        private readonly IMessageBus _messageBus;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;
        private readonly ILogService _logger;

        public VacancySummaryPageConsumerAsync(IMessageBus messageBus, IVacancySummaryProcessor vacancySummaryProcessor, ILogService logger)
        {
            _messageBus = messageBus;
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _logger = logger;
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
                _logger.Info("Vacancy ETL Queue completed: {0} vacancy summary pages queued ", vacancySummaryPage.TotalPages);

                _logger.Info("Publishing VacancySummaryUpdateComplete message to queue");
                
                var vsuc = new VacancySummaryUpdateComplete
                {
                    ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime
                };

                _messageBus.PublishMessage(vsuc);

                _logger.Info("Published VacancySummaryUpdateComplete message published to queue");
            }
        }
    }
}
