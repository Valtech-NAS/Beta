using SFA.Apprenticeships.Domain.Interfaces.Caching;

namespace SFA.Apprenticeships.Infrastructure.Caching.Azure.IoC
{
    using StructureMap.Configuration.DSL;

    public class AzureCacheRegistry : Registry
    {
        public AzureCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<AzureCacheService>();
        }
    }
}
