namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Messaging
{
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.Azure.Common;
    using SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration;

    public class AzureScheduleQueue : IMessageService<StorageQueueMessage>
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
            var storageMessage = AzureMessageHelper.DeserialiseQueueMessage<StorageQueueMessage>(message);
            storageMessage.MessageId = message.Id;
            return storageMessage;
        }

        public void DeleteMessage(string id)
        {
            _azureCloud.DeleteMessage(_cloudConfig.VacancyScheduleQueueName, id);
        }

        public void AddMessage(StorageQueueMessage queueMessage)
        {
            var cloudMessage = AzureMessageHelper.SerialiseQueueMessage(queueMessage);
            _azureCloud.AddMessage(_cloudConfig.VacancyScheduleQueueName, cloudMessage);
        }
    }
}
