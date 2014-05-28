namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;

    public class CachedReferenceDataService : IReferenceDataService
    {
        private static readonly ReferenceDataServiceCacheKeyEntry OccupationsCacheKeyEntry = new ReferenceDataServiceCacheKeyEntry("Occupations");
        private static readonly ReferenceDataServiceCacheKeyEntry FrameworksCacheKeyEntry = new ReferenceDataServiceCacheKeyEntry("Frameworks");
        private static readonly ReferenceDataServiceCacheKeyEntry CountriesCacheKeyEntry = new ReferenceDataServiceCacheKeyEntry("Countries");
        private static readonly ReferenceDataServiceCacheKeyEntry ErrorCodeCacheKeyEntry = new ReferenceDataServiceCacheKeyEntry("ErrorCodes");
        private static readonly ReferenceDataServiceCacheKeyEntry LocalAuthCacheKeyEntry = new ReferenceDataServiceCacheKeyEntry("LocalAuthorities");
        private static readonly ReferenceDataServiceCacheKeyEntry RegionCacheKeyEntry = new ReferenceDataServiceCacheKeyEntry("Regions");

        private readonly IReferenceDataService _service;
        private readonly ICacheService _cache;

        public CachedReferenceDataService(ICacheService cache, IReferenceDataService service)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _cache = cache;
            _service = service;
        }

        public IEnumerable<Occupation> GetApprenticeshipOccupations()
        {
            return _cache.Get(OccupationsCacheKeyEntry, _service.GetApprenticeshipOccupations);
        }

        public IEnumerable<Framework> GetApprenticeshipFrameworks()
        {
            return _cache.Get(FrameworksCacheKeyEntry, _service.GetApprenticeshipFrameworks);
        }

        public IEnumerable<County> GetCounties()
        {
            return _cache.Get(CountriesCacheKeyEntry, _service.GetCounties);
        }

        public IEnumerable<ErrorCode> GetErrorCodes()
        {
            return _cache.Get(ErrorCodeCacheKeyEntry, _service.GetErrorCodes);
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities()
        {
            return _cache.Get(LocalAuthCacheKeyEntry, _service.GetLocalAuthorities);
        }

        public IEnumerable<Region> GetRegions()
        {
            return _cache.Get(RegionCacheKeyEntry, _service.GetRegions);
        }
    }
}
