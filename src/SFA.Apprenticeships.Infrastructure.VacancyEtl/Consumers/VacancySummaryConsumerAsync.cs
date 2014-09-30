namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using VacancyIndexer;

    public class VacancySummaryConsumerAsync : IConsumeAsync<VacancySummaryUpdate>
    {
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
                _vacancyIndexer.Index(vacancySummaryToIndex);

                throw new Exception("Something terrible occurred while indexing");
            });
        }
    }
}
