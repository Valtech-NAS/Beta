namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Entities;
    using NLog;
    using VacancyIndexer;

    public class TraineeshipsSummaryUpdateConsumerAsync : IConsumeAsync<TraineeshipSummaryUpdate>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _vacancyIndexer;

        public TraineeshipsSummaryUpdateConsumerAsync(IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> vacancyIndexer)
        {
            _vacancyIndexer = vacancyIndexer;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "TraineeshipsSummaryUpdateConsumerAsync")]
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
                    var message = string.Format("Failed indexing traineeship vacancy summary {0}", vacancySummaryToIndex.Id);
                    Logger.Warn(message, ex);
                }
            });
        }
    }
}
