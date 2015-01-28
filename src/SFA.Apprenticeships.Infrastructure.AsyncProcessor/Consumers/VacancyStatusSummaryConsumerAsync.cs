namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Caching;
    using EasyNetQ.AutoSubscribe;
    using Extensions;
    using NLog;

    public class VacancyStatusSummaryConsumerAsync : IConsumeAsync<VacancyStatusSummary>
    {
        private readonly ICacheService _cacheService;
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                try
                {
                    var cachedSummaryUpdate = _cacheService.Get<VacancyStatusSummary>(message.CacheKey());

                    if (cachedSummaryUpdate != null)
                    {
                        return;
                    }

                    _cacheService.PutObject(message.CacheKey(), message, message.CacheDuration());
                    _applicationStatusProcessor.ProcessApplicationStatuses(message.LegacyVacancyId, message.VacancyStatus, message.ClosingDate);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error processing application summaries", ex);
                }
            });
        }
    }
}
