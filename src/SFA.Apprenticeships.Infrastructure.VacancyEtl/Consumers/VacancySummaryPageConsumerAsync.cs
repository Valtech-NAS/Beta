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
        private readonly IMessageBus _bus;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public VacancySummaryPageConsumerAsync(IMessageBus bus, IVacancySummaryProcessor vacancySummaryProcessor)
        {
            _bus = bus;
            _vacancySummaryProcessor = vacancySummaryProcessor;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryPageConsumerAsync")]
        public Task Consume(VacancySummaryPage vacancySummaryPage)
        {
            return Task.Run(() => ConsumeTask(vacancySummaryPage));
        }

        private void ConsumeTask(VacancySummaryPage vacancySummaryPage)
        {
            Logger.Debug("Queueing Vacancy Etl Index Page {0} of {1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);
            _vacancySummaryProcessor.QueueVacancySummaries(vacancySummaryPage);
            Logger.Debug("Queued Vacancy Etl Index Page {0} of {1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);

            if (vacancySummaryPage.PageNumber == vacancySummaryPage.TotalPages)
            {
                var vsuc = new VacancySummaryUpdateComplete()
                {
                    ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime
                };
                _bus.PublishMessage(vsuc);
            }
        }
    }
}
