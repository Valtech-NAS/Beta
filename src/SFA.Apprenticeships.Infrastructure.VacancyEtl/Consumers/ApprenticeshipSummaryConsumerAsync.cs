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

    public class ApprenticeshipSummaryConsumerAsync : IConsumeAsync<ApprenticeshipSummaryUpdate>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _vacancyIndexer;
        private readonly IMessageBus _messageBus;

        public ApprenticeshipSummaryConsumerAsync(IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> vacancyIndexer, 
            IMessageBus messageBus)
        {
            _vacancyIndexer = vacancyIndexer;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "ApprenticeshipSummaryConsumerAsync")]
        public Task Consume(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
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
