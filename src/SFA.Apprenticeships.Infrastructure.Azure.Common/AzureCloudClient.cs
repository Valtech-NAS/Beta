namespace SFA.Apprenticeships.Infrastructure.Azure.Common
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Configuration;

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

        public void DeleteMessage(string queueName, string id, string popReceipt)
        {
            _cloudQueueClient.GetQueueReference(queueName).DeleteMessage(id, popReceipt);
        }

        public void AddMessage(string queueName, CloudQueueMessage queueMessage)
        {
            _cloudQueueClient.GetQueueReference(queueName).AddMessage(queueMessage);
        }
    }
}
