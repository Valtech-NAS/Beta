namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using Domain.Interfaces.Messaging;
    using Messaging;
    using NLog;
    using Tasks;

    public class MonitorSchedulerConsumer
    {
        private readonly IProcessControlQueue<StorageQueueMessage> _messageService;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMonitorTasksRunner _monitorTasksRunner;

        public MonitorSchedulerConsumer(IProcessControlQueue<StorageQueueMessage> messageService, IMonitorTasksRunner monitorTasksRunner)
        {
            _messageService = messageService;
            _monitorTasksRunner = monitorTasksRunner;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() => CheckQueue());
        }

        private void CheckQueue()
        {
            var latestScheduledMessage = GetLatestQueueMessage();

            if (latestScheduledMessage != null)
            {
                Logger.Info("Found Monitor control message, starting monitor process");

                _monitorTasksRunner.RunMonitorTasks();
            }
        }

        private StorageQueueMessage GetLatestQueueMessage()
        {
            Logger.Debug("Checking Monitor control queue");
            var queueMessage = _messageService.GetMessage();

            if (queueMessage == null)
            {
                return null;
            }

            while (true)
            {
                var nextQueueMessage = _messageService.GetMessage();
                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                Logger.Warn("Found more than 1 Monitor control message on queue");
                _messageService.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
            }

            return queueMessage;
        }
    }
}
