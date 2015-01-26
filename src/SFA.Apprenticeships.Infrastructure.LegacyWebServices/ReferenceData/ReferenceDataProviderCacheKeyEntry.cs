namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using Domain.Interfaces.Caching;

    public class ReferenceDataProviderCacheKeyEntry : BaseCacheKey
    {
        private const string ReferenceDataProviderCacheKey = "SFA.Apprenticeships.ReferenceData";

        protected override string KeyPrefix
        {
            get { return ReferenceDataProviderCacheKey; }
        }

        public override CacheDuration Duration
        {
            get { return CacheDuration.OneHour; }
        }
    }
}
