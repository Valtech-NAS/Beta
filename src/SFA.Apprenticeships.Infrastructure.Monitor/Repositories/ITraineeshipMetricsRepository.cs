namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface ITraineeshipMetricsRepository
    {
        long GetApplicationCount();
        long GetApplicationsPerCandidateCount();
    }
}
