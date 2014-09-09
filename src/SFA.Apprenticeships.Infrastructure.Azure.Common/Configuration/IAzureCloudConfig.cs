namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Configuration
{
    public interface IAzureCloudConfig
    {
        string StorageConnectionString { get; }
        string QueueName { get; }
    }
}
