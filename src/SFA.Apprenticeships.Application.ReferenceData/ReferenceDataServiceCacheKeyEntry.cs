
namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;

    public class ReferenceDataServiceCacheKeyEntry : BaseCacheKey
    {
        private const string ReferenceDataServiceCacheKey = "SFA.Apprenticeships.LegacyReferenceData.";
        private string _subKey;

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
