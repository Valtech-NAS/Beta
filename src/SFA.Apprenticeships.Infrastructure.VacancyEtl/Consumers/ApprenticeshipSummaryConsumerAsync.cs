namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Entities;
    using NLog;
    using Application.VacancyEtl;
    using VacancyIndexer;

    public class ApprenticeshipSummaryConsumerAsync : IConsumeAsync<ApprenticeshipSummaryUpdate>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _vacancyIndexer;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;

        public ApprenticeshipSummaryConsumerAsync(IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> vacancyIndexer, 
            IVacancySummaryProcessor vacancySummaryProcessor)
        {
            _vacancyIndexer = vacancyIndexer;
            _vacancySummaryProcessor = vacancySummaryProcessor;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApprenticeshipSummaryConsumerAsync")]
        public Task Consume(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                try
                {
                    _vacancyIndexer.Index(vacancySummaryToIndex);
                    _vacancySummaryProcessor.QueueVacancyIfExpiring(vacancySummaryToIndex);
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
