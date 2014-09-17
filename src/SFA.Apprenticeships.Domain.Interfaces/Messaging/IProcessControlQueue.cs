namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    using System;

    public interface IProcessControlQueue<T> where T : StorageQueueMessage
    {
        T GetMessage();

        void DeleteMessage(string messageId, string popReceipt);
    }
}
