namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    public interface IPerformanceCounterService
    {
        void IncrementCounter(string categoryName, string performanceCounterName);
    }
}