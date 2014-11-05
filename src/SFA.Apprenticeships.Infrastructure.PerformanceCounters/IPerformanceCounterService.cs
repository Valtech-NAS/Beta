namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    public interface IPerformanceCounterService
    {
        void IncrementCandidateRegistrationCounter();

        void IncrementApplicationSubmissionCounter();
        void IncrementVacancySearchPerformanceCounter();
        void IncrementVacancyIndexPerformanceCounter();
    }
}