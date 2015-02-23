namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface IUserMetricsRepository
    {
        long GetRegisteredUserCount();
        long GetRegisteredAndActivatedUserCount();
    }
}
