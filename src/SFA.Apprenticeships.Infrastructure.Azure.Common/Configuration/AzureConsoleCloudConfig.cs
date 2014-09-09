namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Domain.Interfaces.Configuration;

    public class AzureConsoleCloudConfig : IAzureCloudConfig
    {
        public AzureConsoleCloudConfig(IConfigurationManager configurationManager)
        {
            StorageConnectionString = configurationManager.GetAppSetting<string>("StorageConnectionString");
            QueueName = configurationManager.GetAppSetting<string>("QueueName");
        }

        public string StorageConnectionString { get; private set; }

        public string QueueName { get; private set; }
    }
}
