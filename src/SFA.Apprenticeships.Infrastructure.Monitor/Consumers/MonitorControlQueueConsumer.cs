using System;

namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;
    using SFA.Apprenticeships.Domain.Interfaces.Messaging;
    using SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging;
    using SFA.Apprenticeships.Infrastructure.Monitor.Tasks;

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

        private static bool IsMonitorEnabled()
        {
            bool monitorEnabled;
            bool.TryParse(CloudConfigurationManager.GetSetting("MonitorEnabled"), out monitorEnabled);
            return monitorEnabled;
        }
    }
}