using System;
using System.Collections.Generic;
using System.Linq;
using SFA.Apprenticeships.Common.Caching;
using SFA.Apprenticeships.Services.Models.ReferenceData;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Models;
using SFA.Apprenticeships.Web.Common.Models.Common;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    public class LegacyReferenceDataProvider : IReferenceDataProvider
    {
        public const string LegacyReferenceDataCacheKey = "SFA.Apprenticeships.LegacyReferenceData.";

        private readonly IReferenceDataService _service;
        private readonly ICacheClient _cache;

        public LegacyReferenceDataProvider(IReferenceDataService service, ICacheClient cache = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _service = service;
            _cache = cache;
        }

        public IEnumerable<ReferenceDataViewModel> Get(LegacyReferenceDataType type)
        {
            var result = new List<ReferenceDataViewModel>();
            var cachedData = default(IList<ILegacyReferenceData>);

            if (_cache != null)
            {
                cachedData = _cache.Get<IList<ILegacyReferenceData>>(new LegacyDataProviderCacheKeyEntry().Key(type));           
            }

            // No cache data found then call the service
            if (cachedData == null || !cachedData.Any())
            {
                cachedData = new List<ILegacyReferenceData>();

                var serviceData = _service.GetReferenceData(type);
                if (serviceData != null)
                {
                    (cachedData as List<ILegacyReferenceData>).AddRange(serviceData);

                    if (_cache != null)
                    {
                        _cache.Put(new LegacyDataProviderCacheKeyEntry(), cachedData, type);
                    }
                }
            }

            // Create the view model
            result.AddRange(
                cachedData
                    .Select(
                        item =>
                            new ReferenceDataViewModel
                            {
                                Id = item.Id,
                                Description = item.Description
                            }));

            return result;
        }
    }
}
