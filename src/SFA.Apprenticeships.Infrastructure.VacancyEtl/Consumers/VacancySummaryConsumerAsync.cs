namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using NLog;
    using VacancyIndexer;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummaryUpdate>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService _vacancyIndexer;
        private readonly IMessageBus _messageBus;

        public VacancySummaryConsumerAsync(IVacancyIndexerService vacancyIndexer, 
            IMessageBus messageBus)
        {
            _vacancyIndexer = vacancyIndexer;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerAsync")]
        public Task Consume(VacancySummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                //Logger.Debug("Vacancy summary update calling vacancy indexer index VacancyId={0}", vacancySummaryToIndex.Id);
                try
                {
                    _vacancyIndexer.Index(vacancySummaryToIndex);
                }
                catch(Exception ex)
                {
                    var message = string.Format("Failed indexing vacancy summary {0}. Requeuing.",
                        vacancySummaryToIndex.Id);
                    Logger.Error(message, ex);

                    _messageBus.PublishMessage(vacancySummaryToIndex);
                }

                //Logger.Debug("Vacancy summary update indexed VacancyId={0}", vacancySummaryToIndex.Id);
            });
        }
    }
}
