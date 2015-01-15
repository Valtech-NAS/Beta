namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using Extensions;
    using NLog;

    public class VacancyStatusSummaryConsumerAsync : IConsumeAsync<VacancyStatusSummary>
    {
        private readonly ICacheService _cacheService;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IMessageBus _bus;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public VacancyStatusSummaryConsumerAsync(ICacheService cacheService,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IMessageBus bus
            )
        {
            _cacheService = cacheService;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _bus = bus;
        }

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
                    var applicationStatusSummaries =
                        _apprenticeshipApplicationReadRepository.GetApprenticeshipApplications(message.LegacyVacancyId,
                            message.VacancyStatus);

                    Parallel.ForEach(
                        applicationStatusSummaries,
                        new ParallelOptions {MaxDegreeOfParallelism = 5},
                        applicationStatusSummary => _bus.PublishMessage(applicationStatusSummary));
                }
                catch (Exception ex)
                {
                    Logger.Error("Error sending email", ex);
                }
            });
        }
    }
}
