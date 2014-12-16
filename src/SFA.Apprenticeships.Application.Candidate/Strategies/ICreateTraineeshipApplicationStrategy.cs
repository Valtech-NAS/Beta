namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public interface ICreateTraineeshipApplicationStrategy
    {
        TraineeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId);
    }
}
