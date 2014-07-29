namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Applications;
    using Entities;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatuses(StorageQueueMessage scheduledQueueMessage);

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary);
    }
}
