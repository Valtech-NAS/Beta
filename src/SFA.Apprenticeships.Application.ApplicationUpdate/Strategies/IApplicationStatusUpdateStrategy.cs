namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Entities.Applications;

    public interface IApplicationStatusUpdateStrategy
    {
        void Update(ApplicationStatusSummary applicationStatus);
    }
}
