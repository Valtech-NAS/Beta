namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;

    public abstract class AzureControlQueueConsumer
    {
        private readonly ILogService _logger;
        protected readonly IProcessControlQueue<StorageQueueMessage> MessageService;
        private readonly string _processName;

        protected AzureControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService, ILogService logger, string processName)
        {
            MessageService = messageService;
            _processName = processName;
            _logger = logger;
        }

        protected StorageQueueMessage GetLatestQueueMessage()
        {
            _logger.Debug("Checking control queue for " + _processName + " process");
            var queueMessage = MessageService.GetMessage();

            if (queueMessage == null)
            {
                _logger.Debug("No control message found for " + _processName);
                return null;
            }

            var foundSurplusMessages = false;

            while (true)
            {
                var nextQueueMessage = MessageService.GetMessage();
                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                MessageService.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
                foundSurplusMessages = true;
            }

            if (foundSurplusMessages)
            {
                _logger.Warn("Found more than 1 control message for " + _processName + " process");
            }

            _logger.Info("Found valid control message to start " + _processName + " process");

            return queueMessage;
        }
    }
}
