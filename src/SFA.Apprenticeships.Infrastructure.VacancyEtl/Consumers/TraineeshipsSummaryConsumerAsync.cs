namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Entities;
    using NLog;
    using VacancyIndexer;

    public class TraineeshipsSummaryConsumerAsync : IConsumeAsync<TraineeshipSummaryUpdate>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _vacancyIndexer;
        private readonly IMessageBus _messageBus;

        public TraineeshipsSummaryConsumerAsync(IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> vacancyIndexer, 
            IMessageBus messageBus)
        {
            _vacancyIndexer = vacancyIndexer;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "TraineeshipSummaryConsumerAsync")]
        public Task Consume(TraineeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                try
                {
                    _vacancyIndexer.Index(vacancySummaryToIndex);
                }
                catch(Exception ex)
                {
                    var message = string.Format("Failed indexing vacancy summary {0}", vacancySummaryToIndex.Id);
                    Logger.Error(message, ex);
                }
            });
        }
    }
}
