namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading.Tasks;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.ReferenceData;
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
        private readonly IReferenceDataService _referenceDataService;
        static readonly object CacheLock = new object();

        public ApprenticeshipSummaryConsumerAsync(IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> vacancyIndexer, 
            IVacancySummaryProcessor vacancySummaryProcessor, IReferenceDataService referenceDataService)
        {
            _vacancyIndexer = vacancyIndexer;
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _referenceDataService = referenceDataService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApprenticeshipSummaryConsumerAsync")]
        public Task Consume(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                try
                {
                    PopulateCategoriesCodes(vacancySummaryToIndex);

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

        private void PopulateCategoriesCodes(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            // based on: http://stackoverflow.com/questions/21269170/locking-pattern-for-proper-use-of-net-memorycache
            var cache = MemoryCache.Default;
            const string categoriesCacheKey = "categories";

            var categories = (IEnumerable<Category>) cache.Get(categoriesCacheKey);

            if (categories == null)
            {
                lock (CacheLock)
                {
                    categories = (IEnumerable<Category>)cache.Get(categoriesCacheKey);

                    if (categories == null)
                    {
                        categories = _referenceDataService.GetCategories();
                        var cacheItemPolicy = new CacheItemPolicy
                        {
                            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(20))
                        };

                        cache.Add(categoriesCacheKey, categories, cacheItemPolicy);
                    }
                }
            }

            vacancySummaryToIndex.SectorCode =
                categories.First(c => c.FullName == vacancySummaryToIndex.Sector).CodeName;

            vacancySummaryToIndex.FrameworkCode =
                categories.First(c => c.FullName == vacancySummaryToIndex.Sector)
                    .SubCategories.First(sc => sc.FullName == vacancySummaryToIndex.Framework)
                    .CodeName;
        }
    }
}
