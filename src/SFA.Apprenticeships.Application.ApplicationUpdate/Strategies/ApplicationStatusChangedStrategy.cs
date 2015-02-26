namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Entities;

    public class ApplicationStatusChangedStrategy : IApplicationStatusChangedStrategy
    {
        public void Send(ApplicationStatusSummary applicationStatusSummary)
        {
            //todo: 1.7: publish communication message for later processing. details TBC - may be removed...
        }
    }
}
