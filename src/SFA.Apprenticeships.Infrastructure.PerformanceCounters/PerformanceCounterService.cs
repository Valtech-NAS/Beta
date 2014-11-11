namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    using System.Diagnostics;

    public class PerformanceCounterService : IPerformanceCounterService
    {
        private const string VacancyEtlPerformanceCounterCategory = "SFA.Apprenticeships.WorkerRoles.VacancyEtl";
        
        
        
        public void IncrementVacancyIndexPerformanceCounter()
        {
            IncrementVacancyEtlCounter(VacancyIndexCounter);
        }

        private void IncrementVacancyEtlCounter(string performanceCounterName)
        {
            IncrementCounter(VacancyEtlPerformanceCounterCategory, performanceCounterName);
        }

        public void IncrementCounter(string categoryName, string performanceCounterName)
        {
            using (var counter = new PerformanceCounter(categoryName, performanceCounterName, false))
            {
                counter.Increment();
            }
        }
    }
}