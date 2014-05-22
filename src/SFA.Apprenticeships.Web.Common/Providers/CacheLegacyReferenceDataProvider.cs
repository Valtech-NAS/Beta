namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Apprenticeships.Common.Caching;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;

    public class CacheLegacyReferenceDataProvider : IReferenceDataProvider
    {
        public const string LegacyReferenceDataCacheKey = "SFA.Apprenticeships.LegacyReferenceData.";

        private readonly IReferenceDataProvider _provider;
        private readonly ICacheClient _cache;

        public CacheLegacyReferenceDataProvider(ICacheClient cache, IReferenceDataProvider provider)
        {           
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            _provider = provider;
            _cache = cache;
        }

        public IEnumerable<ReferenceDataViewModel> Get(LegacyReferenceDataType type)
        {
            var data = _cache.Get<IEnumerable<ReferenceDataViewModel>>(new LegacyDataProviderCacheKeyEntry().Key(type));           

            // No cache data found then call the service
            if (data == null || !data.Any())
            {
                data = _provider.Get(type);
                if (data != null && data.Any())
                {
                    _cache.Put(new LegacyDataProviderCacheKeyEntry(), data, type);
                }
            }

            return data;
        }
    }
}
