namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using System;
    using Entities;

    public interface IApplicationStatusChangedStrategy
    {
        void Send(ApplicationStatusSummary applicationStatusSummary);
    }
}
