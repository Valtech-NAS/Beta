namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IProcessControlQueue<T>
    {
        T GetMessage();

        void DeleteMessage(string id, string popReceipt);

        void AddMessage(T queueMessage);
    }
}
