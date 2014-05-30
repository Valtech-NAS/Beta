namespace SFA.Apprenticeships.Infrastructure.Azure.Common
{
    using Microsoft.WindowsAzure.Storage.Queue;

    /// <summary>
    /// Simple wrapper to abstract azure queue service to aid abstraction / testing
    /// </summary>
    public interface IAzureCloudClient
    {
        CloudQueueMessage GetMessage(string queueName);

        void DeleteMessage(string queueName, string id);

        void AddMessage(string queueName, CloudQueueMessage queueMessage);
    }
}
