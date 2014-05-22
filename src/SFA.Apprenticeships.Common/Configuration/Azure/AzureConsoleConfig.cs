namespace SFA.Apprenticeships.Common.Configuration.Azure
{
    public class AzureConsoleConfig : IAzureCloudConfig
    {
        public AzureConsoleConfig(IConfigurationManager configManager)
        {
            StorageConnectionString = configManager.GetAppSetting("StorageConnectionString");
        }

        public string StorageConnectionString { get; private set; }
    }
}
