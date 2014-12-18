namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Applications;

    public interface ISaveTraineeshipApplicationStrategy
    {
        TraineeshipApplicationDetail SaveApplication(TraineeshipApplicationDetail traineeshipApplicationDetail);
    }
}