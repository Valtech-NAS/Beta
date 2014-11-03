namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    public interface IPerformanceCounterService
    {
        void IncrementCandidateRegistrationCounter();

        void IncrementApplicationSubmissionCounter();
    }
}