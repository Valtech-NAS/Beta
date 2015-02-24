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

        public StorageQueueMessage GetMessage(string queueName)
        {
            // Queue name can be overridden by caller but typically comes from cloud configuration.
            queueName = queueName ?? _azureCloudConfig.QueueName;

            _logger.Debug("Checking Azure control queue for control message: '{0}'", queueName);

            // If queue name is not specified, get it from configuration.
            var message = _azureCloudClient.GetMessage(queueName ?? _azureCloudConfig.QueueName);

            if (message == null)
            {
                _logger.Debug("Azure control queue empty: '{0}'", queueName);
                return null;
            }

            _logger.Debug("Azure control queue item returned: '{0}'", queueName);

            var storageMessage = AzureMessageHelper.DeserialiseQueueMessage<StorageQueueMessage>(message);
            storageMessage.MessageId = message.Id;
            storageMessage.PopReceipt = message.PopReceipt;

            _logger.Debug("Azure control queue item deserialised: '{0}'", queueName);

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
