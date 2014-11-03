namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters.IoC
{
    using StructureMap.Configuration.DSL;

    public class PerformanceCounterRegistry : Registry
    {
        public PerformanceCounterRegistry()
        {
            For<IPerformanceCounterService>().Use<PerformanceCounterService>();    
        }
    }
}