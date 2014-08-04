namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Messaging
{
    using System;
    using Application.ApplicationUpdate.Entities;
    using Domain.Interfaces.Messaging;

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
