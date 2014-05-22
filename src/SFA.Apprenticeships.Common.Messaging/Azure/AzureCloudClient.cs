namespace SFA.Apprenticeships.Common.Messaging.Azure
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using SFA.Apprenticeships.Common.Configuration.Azure;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;

    public class AzureCloudClient : IAzureCloudClient
    {
        private readonly CloudQueueClient _cloudQueueClient;

        public AzureCloudClient(IAzureCloudConfig cloudConfig)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(cloudConfig.StorageConnectionString);
            _cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
        }

        public CloudQueueMessage GetMessage(string queueName)
        {
            return _cloudQueueClient.GetQueueReference(queueName).GetMessage();    
        }

        public void DeleteMessage(string queueName, CloudQueueMessage queueMessage)
        {
            _cloudQueueClient.GetQueueReference(queueName).DeleteMessage(queueMessage);
        }

        public void AddMessage(string queueName, CloudQueueMessage queueMessage)
        {
            _cloudQueueClient.GetQueueReference(queueName).AddMessage(queueMessage);
        }
    }
}
