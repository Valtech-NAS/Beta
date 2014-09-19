namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    public interface IMonitorTask
    {
        string TaskName { get; }

        void Run();
    }
}
