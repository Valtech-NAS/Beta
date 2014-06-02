namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System;
    using Domain.Interfaces.Services.Caching;

    public class ReferenceDataServiceCacheKeyEntry : BaseCacheKey
    {
        private const string ReferenceDataServiceCacheKey = "SFA.Apprenticeships.LegacyReferenceData.";
        private readonly string _subKey;

        public ReferenceDataServiceCacheKeyEntry(string subKey)
        {
            _subKey = subKey;
        }

        protected override string KeyPrefix
        {
            get { return ReferenceDataServiceCacheKey + _subKey; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.OneDay; }
        }
    }
}
