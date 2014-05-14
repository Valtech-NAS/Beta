using SFA.Apprenticeships.Common.Caching;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    public class LegacyDataProviderCacheKeyEntry : BaseCacheEntry
    {
        protected override string KeyPrefix
        {
            get { return LegacyReferenceDataProvider.LegacyReferenceDataCacheKey; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.OneDay; }
        }
    }
}
