namespace SFA.Apprenticeships.Infrastructure.Azure.Common
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Configuration;
    using NLog;

    public class AzureCloudClient : IAzureCloudClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly CloudQueueClient _cloudQueueClient;

        public AzureCloudClient(IAzureCloudConfig cloudConfig)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(cloudConfig.StorageConnectionString);
            _cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
        }

        public CloudQueueMessage GetMessage(string queueName)
        {
            Logger.Debug("Getting message");
            return _cloudQueueClient.GetQueueReference(queueName).GetMessage();    
        }

        public void DeleteMessage(string queueName, string id, string popReceipt)
        {
            Logger.Debug("Deleting message");
            _cloudQueueClient.GetQueueReference(queueName).DeleteMessage(id, popReceipt);
        }

        public void AddMessage(string queueName, CloudQueueMessage queueMessage)
        {
            Logger.Debug("Adding message");
            _cloudQueueClient.GetQueueReference(queueName).AddMessage(queueMessage);
        }
    }
}
