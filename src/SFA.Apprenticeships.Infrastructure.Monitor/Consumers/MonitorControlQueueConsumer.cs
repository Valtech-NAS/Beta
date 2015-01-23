namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Azure.Common.Messaging;
    using Tasks;

    public class MonitorControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IMonitorTasksRunner _monitorTasksRunner;
        private readonly IConfigurationManager _configurationManager;

        public MonitorControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            IMonitorTasksRunner monitorTasksRunner, IConfigurationManager configurationManager) : base(messageService, "Monitor")
        {
            _monitorTasksRunner = monitorTasksRunner;
            _configurationManager = configurationManager;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var monitorScheduleMessage = GetLatestQueueMessage();
                if (monitorScheduleMessage != null)
                {
                    if (IsMonitorEnabled())
                    {
                        _monitorTasksRunner.RunMonitorTasks();
                    }
                    _messageService.DeleteMessage(monitorScheduleMessage.MessageId, monitorScheduleMessage.PopReceipt);
                }
            });
        }

        private bool IsMonitorEnabled()
        {   
            return _configurationManager.GetCloudAppSetting<bool>("MonitorEnabled");
        }
    }
}