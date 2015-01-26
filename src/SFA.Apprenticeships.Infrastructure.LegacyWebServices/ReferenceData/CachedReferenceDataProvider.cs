namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System.Collections.Generic;
    using Application.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Caching;
    using NLog;

    public class CachedReferenceDataProvider : IReferenceDataProvider
    {
        private static readonly BaseCacheKey CacheKey = new ReferenceDataProviderCacheKeyEntry();
        private readonly IReferenceDataProvider _legcayService;
        private readonly ICacheService _cacheService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public CachedReferenceDataProvider(IReferenceDataProvider legcayService, ICacheService cacheService)
        {
            _legcayService = legcayService;
            _cacheService = cacheService;
        }

        public IEnumerable<Category> GetCategories()
        {
            Logger.Debug("Calling cached GetCategories");
            return _cacheService.Get(CacheKey, _legcayService.GetCategories);
        }
    }
}
