namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System;
    using Domain.Interfaces.Services.Caching;

    public class ReferenceDataServiceCacheKeyEntry : BaseCacheKey
    {
        private const string ReferenceDataServiceCacheKey = "SFA.Apprenticeships.LegacyReferenceData";

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
