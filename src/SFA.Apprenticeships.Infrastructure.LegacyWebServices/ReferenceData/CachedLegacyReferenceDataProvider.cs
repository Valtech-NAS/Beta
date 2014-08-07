namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using System.Collections.Generic;
    using Application.Interfaces.ReferenceData;
    using Application.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Caching;
    using NLog;

    public class CachedLegacyReferenceDataProvider : IReferenceDataProvider //todo: may be redundant TBC
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly BaseCacheKey CacheKey = new ReferenceDataServiceCacheKeyEntry();
        private readonly ICacheService _cache;
        private readonly IReferenceDataProvider _referenceDataProvider;

        public CachedLegacyReferenceDataProvider(ICacheService cache, IReferenceDataProvider referenceDataProvider)
        {
            _cache = cache;
            _referenceDataProvider = referenceDataProvider;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            Logger.Debug("GetReferenceData called for CacheKey={0}, Type={1}", CacheKey.Key(), type);

            var items = _cache.Get(CacheKey, _referenceDataProvider.GetReferenceData, type);

            Logger.Debug("Successfully returned items for {0}", type);

            return items;
        }
    }
}