namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    using System.Diagnostics;

    public class PerformanceCounterService : IPerformanceCounterService
    {
        public void IncrementCounter(string categoryName, string performanceCounterName)
        {
            using (var counter = new PerformanceCounter(categoryName, performanceCounterName, false))
            {
                counter.Increment();
            }
        }
    }
}