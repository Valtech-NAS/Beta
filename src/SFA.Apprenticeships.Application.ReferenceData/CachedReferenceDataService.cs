namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Interfaces.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Caching;

    public class CachedReferenceDataService : IReferenceDataService
    {
        private static readonly BaseCacheKey CacheKey = new ReferenceDataServiceCacheKeyEntry();
        private readonly IReferenceDataService _service;
        private readonly ICacheService _cache;

        public CachedReferenceDataService(ICacheService cache, IReferenceDataService service)
        {
            _cache = cache;
            _service = service;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            return _cache.Get(CacheKey, _service.GetReferenceData, type);
        }
    }
}
