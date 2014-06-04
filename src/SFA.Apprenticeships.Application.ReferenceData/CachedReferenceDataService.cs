using SFA.Apprenticeships.Domain.Interfaces.Caching;

namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Interfaces.ReferenceData;
    using Domain.Entities.ReferenceData;

    public class CachedReferenceDataService : IReferenceDataService
    {
        private static readonly BaseCacheKey CacheKey = new ReferenceDataServiceCacheKeyEntry();
        private readonly IReferenceDataService _service;
        private readonly ICacheService _cache;

        public CachedReferenceDataService(ICacheService cache, IReferenceDataService service)
        {
            Condition.Requires(cache, "cache").IsNotNull();
            Condition.Requires(service, "service").IsNotNull();

            _cache = cache;
            _service = service;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            return _cache.Get(CacheKey, _service.GetReferenceData, type);
        }
    }
}
