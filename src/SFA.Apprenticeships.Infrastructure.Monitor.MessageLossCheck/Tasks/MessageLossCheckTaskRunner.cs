namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System.Collections.Generic;
    using Monitor.Tasks;

    public class MessageLossCheckTaskRunner : IMessageLossCheckTaskRunner
    {
        private readonly IEnumerable<IMonitorTask> _monitorTasks;

        public MessageLossCheckTaskRunner(IEnumerable<IMonitorTask> monitorTasks)
        {
            _monitorTasks = monitorTasks;
        }

        public void RunMonitorTasks()
        {
            foreach (var monitorTask in _monitorTasks)
            {
                monitorTask.Run();
            }
        }
    }
}