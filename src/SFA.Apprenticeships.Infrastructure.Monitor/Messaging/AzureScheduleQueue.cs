namespace SFA.Apprenticeships.Infrastructure.Monitor.Messaging
{
    using System;
    using Domain.Interfaces.Messaging;
    using Consumers;

    public class AzureScheduleQueue : IProcessControlQueue<StorageQueueMessage>
    {
        //todo: maybe refactor this and SFA.Apprenticeships.Infrastructure.VacancyEtl.Messaging.AzureScheduleQueue to shared type
        public StorageQueueMessage GetMessage()
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(string id, string popReceipt)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(StorageQueueMessage queueMessage)
        {
            throw new NotImplementedException();
        }
    }
}
