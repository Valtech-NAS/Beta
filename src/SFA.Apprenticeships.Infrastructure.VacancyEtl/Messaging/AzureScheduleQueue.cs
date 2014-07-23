namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Messaging
{
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Azure.Common;
    using Azure.Common.Configuration;
    using NLog;

    public class AzureScheduleQueue : IProcessControlQueue<StorageQueueMessage>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IAzureCloudClient _azureCloud;
        private readonly IAzureCloudConfig _cloudConfig;

        public AzureScheduleQueue(IAzureCloudClient azureCloud, IAzureCloudConfig cloudConfig)
        {
            _azureCloud = azureCloud;
            _cloudConfig = cloudConfig;
        }

        public StorageQueueMessage GetMessage()
        {
            Logger.Debug("Checking Azure queue for control message");

            var message = _azureCloud.GetMessage(_cloudConfig.VacancyScheduleQueueName);
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

        public void DeleteMessage(string id, string popReceipt)
        {
            Logger.Debug("Deleting Azure control queue item, id:{0}, receipt:{1}", id, popReceipt);

            _azureCloud.DeleteMessage(_cloudConfig.VacancyScheduleQueueName, id, popReceipt);

            Logger.Debug("Deleted Azure control queue item, id:{0}, receipt:{1}", id, popReceipt);
        }

        public void AddMessage(StorageQueueMessage queueMessage)
        {
            var cloudMessage = AzureMessageHelper.SerialiseQueueMessage(queueMessage);
            _azureCloud.AddMessage(_cloudConfig.VacancyScheduleQueueName, cloudMessage);
        }
    }
}
