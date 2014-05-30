namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    using Microsoft.WindowsAzure;

    public class AzureCloudConfig : IAzureCloudConfig
    {
        public AzureCloudConfig()
        {
            StorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            VacancyScheduleQueueName = CloudConfigurationManager.GetSetting("VacancyScheduleQueueName");
        }

        public string StorageConnectionString { get; private set; }


        public string VacancyScheduleQueueName { get; private set; }
    }
}
