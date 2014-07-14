using SFA.Apprenticeships.Infrastructure.Common.Configuration;

namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    public class AzureConsoleConfig : IAzureCloudConfig
    {
        public AzureConsoleConfig(IConfigurationManager configManager)
        {
            StorageConnectionString = configManager.GetAppSetting("StorageConnectionString");
            VacancyScheduleQueueName = configManager.GetAppSetting("VacancyScheduleQueueName");
        }

        public string StorageConnectionString { get; private set; }

        public string VacancyScheduleQueueName { get; private set; }
    }
}
