namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;

    public interface ICreateTraineeshipApplicationStrategy
    {
        TraineeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId);
    }
}
