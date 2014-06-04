namespace SFA.Apprenticeships.Infrastructure.Caching.Azure.IoC
{
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;
    using StructureMap.Configuration.DSL;

    public class AzureCacheRegistry : Registry
    {
        public AzureCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<AzureCacheService>();
        }
    }
}
