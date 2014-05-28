namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Microsoft.WindowsAzure;

    public class AzureCloudConfig : IAzureCloudConfig
    {
        public AzureCloudConfig()
        {
            StorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");    
        }

        public string StorageConnectionString { get; private set; }
    }
}
