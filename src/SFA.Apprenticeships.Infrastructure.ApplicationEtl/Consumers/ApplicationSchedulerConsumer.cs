namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Entities;
    using Domain.Interfaces.Messaging;
    using NLog;

    public class ApplicationSchedulerConsumer
    {
        private readonly IProcessControlQueue<StorageQueueMessage> _messageService;
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ApplicationSchedulerConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            IApplicationStatusProcessor applicationStatusProcessor)
        {
            _messageService = messageService;
            _applicationStatusProcessor = applicationStatusProcessor;
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
                Logger.Info("Found Application ETL control message, starting application update process");

                _applicationStatusProcessor.QueueApplicationStatuses(latestScheduledMessage);
            }
        }

        private StorageQueueMessage GetLatestQueueMessage()
        {
            Logger.Debug("Checking Application ETL control queue");
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

                Logger.Warn("Found more than 1 Application ETL control message on queue");
                _messageService.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
            }

            return queueMessage;
        }
    }
}
