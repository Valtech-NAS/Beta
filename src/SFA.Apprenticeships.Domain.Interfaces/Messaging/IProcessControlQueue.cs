namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IProcessControlQueue<T> where T : StorageQueueMessage
    {
        T GetMessage(string queueName = null);

        void DeleteMessage(string messageId, string popReceipt);
    }
}
