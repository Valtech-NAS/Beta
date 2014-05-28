namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Azure
{
    using Microsoft.WindowsAzure.Storage.Queue;

    /// <summary>
    /// Simple wrapper to abstract azure queue service to aid abstraction / testing
    /// </summary>
    public interface IAzureCloudClient
    {
        CloudQueueMessage GetMessage(string queueName);
        void DeleteMessage(string queueName, CloudQueueMessage queueMessage);

        void AddMessage(string queueName, CloudQueueMessage queueMessage);
    }
}
