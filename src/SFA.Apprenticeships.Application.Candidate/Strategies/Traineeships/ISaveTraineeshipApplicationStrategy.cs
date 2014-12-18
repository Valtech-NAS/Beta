namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using Domain.Entities.Applications;

    public interface ISaveTraineeshipApplicationStrategy
    {
        TraineeshipApplicationDetail SaveApplication(TraineeshipApplicationDetail traineeshipApplicationDetail);
    }
}