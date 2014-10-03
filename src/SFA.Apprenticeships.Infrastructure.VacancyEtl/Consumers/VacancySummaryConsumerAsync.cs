namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using VacancyIndexer;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummaryUpdate>
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexerService _vacancyIndexer;

        public VacancySummaryConsumerAsync(IVacancyIndexerService vacancyIndexer)
        {
            _vacancyIndexer = vacancyIndexer;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerAsync")]
        public Task Consume(VacancySummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                //Logger.Debug("Vacancy summary update calling vacancy indexer index VacancyId={0}", vacancySummaryToIndex.Id);

                _vacancyIndexer.Index(vacancySummaryToIndex);

                //Logger.Debug("Vacancy summary update indexed VacancyId={0}", vacancySummaryToIndex.Id);
            });
        }
    }
}
