namespace SFA.Apprenticeships.Infrastructure.Caching.IoC
{
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;
    using SFA.Apprenticeships.Infrastructure.Caching.Caching;
    using StructureMap.Configuration.DSL;

    public class CachingRegistry : Registry
    {
        public CachingRegistry()
        {
            For<ICacheService>().Singleton().Use<MemoryCacheService>();
        }
    }
}
