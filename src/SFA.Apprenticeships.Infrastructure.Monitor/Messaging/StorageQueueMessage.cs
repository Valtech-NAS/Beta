namespace SFA.Apprenticeships.Infrastructure.Monitor.Messaging
{
    //todo: this is similar to the VacancyEtl.Entities.StorageQueueMessage - maybe refactor to a common type
    using System;

    [Serializable]
    public sealed class StorageQueueMessage
    {
        public string MessageId { get; set; }

        public string PopReceipt { get; set; }

        public Guid ClientRequestId { get; set; }

        public DateTime ExpectedExecutionTime { get; set; }

        public string SchedulerJobId { get; set; }
    }
}
