namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;

    public class AzureConsoleConfig : IAzureCloudConfig
    {
        public AzureConsoleConfig(IConfigurationManager configManager)
        {
            StorageConnectionString = configManager.GetAppSetting("StorageConnectionString");
        }

        public string StorageConnectionString { get; private set; }
    }
}
