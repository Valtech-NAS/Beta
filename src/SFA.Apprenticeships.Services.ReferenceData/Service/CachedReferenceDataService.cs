using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SFA.Apprenticeships.Common.Entities.ReferenceData;
using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;
using SFA.Apprenticeships.Common.Interfaces.ReferenceData;

namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    using SFA.Apprenticeships.Common.Caching;

    public class CachedReferenceDataService : IReferenceDataService
    {
        public const string ReferenceDataServiceCacheKey = "SFA.Apprenticeships.LegacyReferenceData.";

        private readonly IReferenceDataService _service;
        private readonly ICacheClient _cache;

        public CachedReferenceDataService(ICacheClient cache, IReferenceDataService service)
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


        public IEnumerable<ILegacyReferenceData> GetReferenceData(LegacyReferenceDataType type)
        {
            var data = _cache.Get<IEnumerable<ILegacyReferenceData>>(new ReferenceDataServiceCacheKeyEntry().Key(type));

            // No cache data found then call the service
            if (data == null || !data.Any())
            {
                data = _service.GetReferenceData(type);
                if (data != null && data.Any())
                {
                    _cache.Put(new ReferenceDataServiceCacheKeyEntry(), data, type);
                }
            }

            return data;
        }

        public IEnumerable<ILegacyReferenceData> GetApprenticeshipOccupations()
        {
            return GetReferenceData(LegacyReferenceDataType.Occupations);
        }

        public IEnumerable<ILegacyReferenceData> GetApprenticeshipFrameworks()
        {
            return GetReferenceData(LegacyReferenceDataType.Framework);
        }

        public IEnumerable<ILegacyReferenceData> GetCounties()
        {
            return GetReferenceData(LegacyReferenceDataType.County);
        }

        public IEnumerable<ILegacyReferenceData> GetErrorCodes()
        {
            return GetReferenceData(LegacyReferenceDataType.ErrorCode);
        }

        public IEnumerable<ILegacyReferenceData> GetLocalAuthorities()
        {
            return GetReferenceData(LegacyReferenceDataType.LocalAuthority);
        }

        public IEnumerable<ILegacyReferenceData> GetRegions()
        {
            return GetReferenceData(LegacyReferenceDataType.Region);
        }
    }
}
