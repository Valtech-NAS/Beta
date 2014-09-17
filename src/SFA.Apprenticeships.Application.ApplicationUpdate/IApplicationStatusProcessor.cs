namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Applications;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatuses();

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary);
    }
}
