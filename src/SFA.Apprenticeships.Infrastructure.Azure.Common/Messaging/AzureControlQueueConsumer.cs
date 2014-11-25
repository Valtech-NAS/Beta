namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using Domain.Interfaces.Messaging;
    using NLog;

    public abstract class AzureControlQueueConsumer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected readonly IProcessControlQueue<StorageQueueMessage> _messageService;
        private readonly string _processName;

        protected AzureControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService, string processName)
        {
            _messageService = messageService;
            _processName = processName;
        }

        protected StorageQueueMessage GetLatestQueueMessage()
        {
            Logger.Debug("Checking control queue for " + _processName + " process");
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

                Logger.Warn("Found more than 1 control message for " + _processName + " process");
                _messageService.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
            }

            Logger.Info("Found valid control message to start " + _processName + " process");

            return queueMessage;
        }
    }
}
