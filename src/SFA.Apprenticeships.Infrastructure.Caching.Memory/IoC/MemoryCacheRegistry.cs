namespace SFA.Apprenticeships.Infrastructure.Caching.Memory.IoC
{
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;
    using StructureMap.Configuration.DSL;

    public class MemoryCacheRegistry : Registry
    {
        public MemoryCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<MemoryCacheService>();
        }
    }
}
