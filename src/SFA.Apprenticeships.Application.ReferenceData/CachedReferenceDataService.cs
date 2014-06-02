namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Interfaces.ReferenceData;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Services.Caching;

    public class CachedReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataService _service;
        private readonly ICacheService _cache;

        public CachedReferenceDataService(ICacheService cache, IReferenceDataService service)
        {
            Condition.Requires("cache");
            Condition.Requires("service");

            _cache = cache;
            _service = service;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(string type)
        {
            var cacheKey = new ReferenceDataServiceCacheKeyEntry(type);

            return _cache.Get(cacheKey, () => _service.GetReferenceData(type));
        }
    }
}
