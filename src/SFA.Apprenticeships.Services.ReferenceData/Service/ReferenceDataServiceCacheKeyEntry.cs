
namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    using SFA.Apprenticeships.Common.Caching;

    public class ReferenceDataServiceCacheKeyEntry : BaseCacheEntry
    {
        protected override string KeyPrefix
        {
            get { return CachedReferenceDataService.ReferenceDataServiceCacheKey; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.OneDay; }
        }
    }
}
