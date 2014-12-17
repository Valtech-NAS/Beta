namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using SFA.Apprenticeships.Domain.Entities.Applications;

    public interface ISubmitTraineeshipApplicationStrategy
    {
        void SubmitApplication(Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplicationDetail);
    }
}