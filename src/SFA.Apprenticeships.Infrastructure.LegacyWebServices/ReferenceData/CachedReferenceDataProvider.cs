namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System.Collections.Generic;
    using Application.Interfaces.Logging;
    using Application.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Caching;

    public class CachedReferenceDataProvider : IReferenceDataProvider
    {
        private readonly ILogService _logger;

        private static readonly BaseCacheKey CacheKey = new ReferenceDataProviderCacheKeyEntry();
        private readonly IReferenceDataProvider _legcayService;
        private readonly ICacheService _cacheService;


        public CachedReferenceDataProvider(IReferenceDataProvider legcayService, ICacheService cacheService, ILogService logger)
        {
            _legcayService = legcayService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public IEnumerable<Category> GetCategories()
        {
            _logger.Debug("Calling cached GetCategories");
            return _cacheService.Get(CacheKey, _legcayService.GetCategories);
        }
    }
}
