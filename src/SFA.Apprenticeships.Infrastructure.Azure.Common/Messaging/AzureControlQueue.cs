namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using System;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Messaging;

    public class AzureControlQueue : IProcessControlQueue<StorageQueueMessage>
    {
        private readonly ILogService _logger;
        private readonly IAzureCloudClient _azureCloudClient;
        private readonly IAzureCloudConfig _azureCloudConfig;

        public AzureControlQueue(IAzureCloudClient azureCloud, IAzureCloudConfig cloudConfig, ILogService logger)
        {
            _azureCloudClient = azureCloud;
            _azureCloudConfig = cloudConfig;
            _logger = logger;
        }

        public StorageQueueMessage GetMessage()
        {
            _logger.Debug("Checking Azure control queue for control message");

            var message = _azureCloudClient.GetMessage(_azureCloudConfig.QueueName);
            if (message == null)
            {
                _logger.Debug("Azure control queue empty");
                return null;
            }

            _logger.Debug("Azure control queue item returned");

            var storageMessage = AzureMessageHelper.DeserialiseQueueMessage<StorageQueueMessage>(message);
            storageMessage.MessageId = message.Id;
            storageMessage.PopReceipt = message.PopReceipt;

            _logger.Debug("Azure control queue item deserialised");

            return storageMessage;
        }

        public void DeleteMessage(string messageId, string popReceipt)
        {
            _logger.Debug("Deleting Azure control queue item");

            _azureCloudClient.DeleteMessage(_azureCloudConfig.QueueName, messageId, popReceipt);

            _logger.Debug("Deleted Azure control queue item");
        }
    }
}
