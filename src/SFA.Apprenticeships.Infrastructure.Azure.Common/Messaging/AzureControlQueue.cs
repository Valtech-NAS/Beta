namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using System;
    using Configuration;
    using Domain.Interfaces.Messaging;
    using NLog;

    public class AzureControlQueue : IProcessControlQueue<StorageQueueMessage>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IAzureCloudClient _azureCloudClient;
        private readonly IAzureCloudConfig _azureCloudConfig;

        public AzureControlQueue(IAzureCloudClient azureCloud, IAzureCloudConfig cloudConfig)
        {
            _azureCloudClient = azureCloud;
            _azureCloudConfig = cloudConfig;
        }

        public StorageQueueMessage GetMessage()
        {
            Logger.Debug("Checking Azure control queue for control message");

            var message = _azureCloudClient.GetMessage(_azureCloudConfig.QueueName);
            if (message == null)
            {
                Logger.Debug("Azure control queue empty");
                return null;
            }

            Logger.Debug("Azure control queue item returned");

            var storageMessage = AzureMessageHelper.DeserialiseQueueMessage<StorageQueueMessage>(message);
            storageMessage.MessageId = message.Id;
            storageMessage.PopReceipt = message.PopReceipt;

            Logger.Debug("Azure control queue item deserialised");

            return storageMessage;
        }

        public void DeleteMessage(string messageId, string popReceipt)
        {
            Logger.Debug("Deleting Azure control queue item, messageId:{0}, receipt:{1}", messageId, popReceipt);

            _azureCloudClient.DeleteMessage(_azureCloudConfig.QueueName, messageId, popReceipt);

            Logger.Debug("Deleted Azure control queue item, messageId:{0}, receipt:{1}", messageId, popReceipt);
        }
    }
}
