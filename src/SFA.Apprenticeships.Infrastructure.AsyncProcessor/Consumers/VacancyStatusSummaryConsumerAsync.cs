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
                        // this vacancy has already been processed so return to prevent endless reprocessing
                        return;
                    }

                    _cacheService.PutObject(message.CacheKey(), message, message.CacheDuration());
                    _applicationStatusProcessor.ProcessApplicationStatuses(message.LegacyVacancyId, message.VacancyStatus, message.ClosingDate);
                    //todo: move the code below (and private methods) into the application process project. shouldn't be in infrastructure layer
                }
                catch (Exception ex)
                {
                    Logger.Error("Error processing application summaries", ex);
                }
            });
        }
    }
}
