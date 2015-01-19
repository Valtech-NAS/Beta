namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Entities.Applications;
    using Entities;

    public interface IApplicationStatusUpdateStrategy
    {
        bool Update(ApprenticeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatus);
        bool Update(TraineeshipApplicationDetail apprenticeshipApplication, ApplicationStatusSummary applicationStatus);
    }
}
