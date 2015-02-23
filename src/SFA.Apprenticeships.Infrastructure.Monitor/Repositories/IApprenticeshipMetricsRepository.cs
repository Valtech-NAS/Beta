namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using Domain.Entities.Applications;

    public interface IApprenticeshipMetricsRepository
    {
        int GetApplicationCount();
        int GetApplicationStateCount(ApplicationStatuses applicationStatus);
        int GetApplicationCountPerCandidate();
        int GetApplicationStateCountPerCandidate(ApplicationStatuses applicationStatus);
    }
}