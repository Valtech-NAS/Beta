namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    public interface IPerformanceCounterService
    {
        void IncrementCounter(string categoryName, string performanceCounterName);

        void IncrementCandidateRegistrationCounter();

        void IncrementApplicationSubmissionCounter();
        void IncrementVacancySearchPerformanceCounter();
        void IncrementVacancyIndexPerformanceCounter();
    }
}