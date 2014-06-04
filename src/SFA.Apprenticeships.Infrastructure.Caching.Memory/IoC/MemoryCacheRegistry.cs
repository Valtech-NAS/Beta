using SFA.Apprenticeships.Domain.Interfaces.Caching;

namespace SFA.Apprenticeships.Infrastructure.Caching.Memory.IoC
{
    using StructureMap.Configuration.DSL;

    public class MemoryCacheRegistry : Registry
    {
        public MemoryCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<MemoryCacheService>();
        }
    }
}
