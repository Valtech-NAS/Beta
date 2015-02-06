namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;
    using EasyNetQ.AutoSubscribe;
    using Extensions;

    public class VacancyStatusSummaryConsumerAsync : IConsumeAsync<VacancyStatusSummary>
    {
        private readonly ICacheService _cacheService;
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        public VacancyStatusSummaryConsumerAsync(ICacheService cacheService, IApplicationStatusProcessor applicationStatusProcessor)
        {
            _cacheService = cacheService;
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancyStatusSummaryConsumerAsync")]
        public Task Consume(VacancyStatusSummary message)
        {
            return Task.Run(() =>
            {
                var cachedSummaryUpdate = _cacheService.Get<VacancyStatusSummary>(message.CacheKey());

                if (cachedSummaryUpdate != null)
                {
                    return; // this vacancy has already been processed so return to prevent endless reprocessing
                }

                _cacheService.PutObject(message.CacheKey(), message, message.CacheDuration());

                _applicationStatusProcessor.ProcessApplicationStatuses(message.LegacyVacancyId, message.VacancyStatus, message.ClosingDate);
            });
        }
    }
}
