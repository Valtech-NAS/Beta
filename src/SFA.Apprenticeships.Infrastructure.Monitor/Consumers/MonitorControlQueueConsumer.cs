namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;
    using Tasks;

    public class MonitorControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IMonitorTasksRunner _monitorTasksRunner;

        public MonitorControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService, 
            IMonitorTasksRunner monitorTasksRunner) : base(messageService, "Monitor")
        {
            _monitorTasksRunner = monitorTasksRunner;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                if (GetLatestQueueMessage() != null)
                {
                    _monitorTasksRunner.RunMonitorTasks();
                }
            });
        }
    }
}
