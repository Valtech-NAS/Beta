namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    using System;

    [Serializable]
    public class StorageQueueMessage
    {
        public string MessageId { get; set; }

        public string PopReceipt { get; set; }

        public Guid ClientRequestId { get; set; }

        public DateTime ExpectedExecutionTime { get; set; }

        public string SchedulerJobId { get; set; }
    }
}
