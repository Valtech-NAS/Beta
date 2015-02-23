namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface ICommunicationMetricsRepository
    {
        int GetDraftApplicationEmailsSentToday();
    }
}
