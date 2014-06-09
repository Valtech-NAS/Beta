namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Messaging
{
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Azure.Common;
    using Azure.Common.Configuration;

    public class AzureScheduleQueue : IProcessControlQueue<StorageQueueMessage>
    {
        private readonly IAzureCloudClient _azureCloud;
        private readonly IAzureCloudConfig _cloudConfig;

        public AzureScheduleQueue(IAzureCloudClient azureCloud, IAzureCloudConfig cloudConfig)
        {
            _azureCloud = azureCloud;
            _cloudConfig = cloudConfig;
        }

        public StorageQueueMessage GetMessage()
        {
            var message = _azureCloud.GetMessage(_cloudConfig.VacancyScheduleQueueName);
            if (message == null)
            {
                return null;
            }

            var storageMessage = AzureMessageHelper.DeserialiseQueueMessage<StorageQueueMessage>(message);
            storageMessage.MessageId = message.Id;
            storageMessage.PopReceipt = message.PopReceipt;
            return storageMessage;
        }

        public void DeleteMessage(string id, string popReceipt)
        {
            _azureCloud.DeleteMessage(_cloudConfig.VacancyScheduleQueueName, id, popReceipt);
        }

        public void AddMessage(StorageQueueMessage queueMessage)
        {
            var cloudMessage = AzureMessageHelper.SerialiseQueueMessage(queueMessage);
            _azureCloud.AddMessage(_cloudConfig.VacancyScheduleQueueName, cloudMessage);
        }
    }
}
