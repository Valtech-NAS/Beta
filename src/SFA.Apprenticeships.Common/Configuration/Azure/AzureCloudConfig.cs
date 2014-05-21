namespace SFA.Apprenticeships.Common.Configuration.Azure
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
