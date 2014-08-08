namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData
{
    using Domain.Interfaces.Caching;

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
