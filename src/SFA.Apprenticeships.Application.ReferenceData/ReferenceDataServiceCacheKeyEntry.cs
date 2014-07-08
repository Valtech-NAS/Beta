using SFA.Apprenticeships.Domain.Interfaces.Caching;

namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System;

    public class ReferenceDataServiceCacheKeyEntry : BaseCacheKey
    {
        private const string ReferenceDataServiceCacheKey = "SFA.Apprenticeships.ReferenceData";

        protected override string KeyPrefix
        {
            get { return ReferenceDataServiceCacheKey; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.OneDay; }
        }
    }
}
