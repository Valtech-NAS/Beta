namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Microsoft.WindowsAzure;

    public class AzureCloudConfig : IAzureCloudConfig
    {
        public AzureCloudConfig()
        {
            StorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            QueueName = CloudConfigurationManager.GetSetting("QueueName");
        }

        public string StorageConnectionString { get; private set; }

        public string QueueName { get; private set; }
    }
}
