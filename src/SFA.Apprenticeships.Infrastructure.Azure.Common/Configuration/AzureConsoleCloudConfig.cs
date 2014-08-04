namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Domain.Interfaces.Configuration;

    public class AzureConsoleCloudConfig : IAzureCloudConfig
    {
        public AzureConsoleCloudConfig(IConfigurationManager configurationManager)
        {
            StorageConnectionString = configurationManager.GetAppSetting<string>("StorageConnectionString");
            VacancyScheduleQueueName = configurationManager.GetAppSetting<string>("VacancyScheduleQueueName");
        }

        public string StorageConnectionString { get; private set; }

        public string VacancyScheduleQueueName { get; private set; }
    }
}
