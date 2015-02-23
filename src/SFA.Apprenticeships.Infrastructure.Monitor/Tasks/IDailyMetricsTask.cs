namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    public interface IDailyMetricsTask
    {
        string TaskName { get; }

        void Run();
    }
}
