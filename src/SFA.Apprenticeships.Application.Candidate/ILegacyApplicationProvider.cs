namespace SFA.Apprenticeships.Application.Candidate
{
    using Domain.Entities.Applications;

    public interface ILegacyApplicationProvider
    {
        int CreateApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail);
        int CreateApplication(TraineeshipApplicationDetail traineeshipApplicationDetail);
    }
}
