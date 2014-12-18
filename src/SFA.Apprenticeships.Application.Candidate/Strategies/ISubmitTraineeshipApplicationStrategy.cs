namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface ISubmitTraineeshipApplicationStrategy
    {
        void SubmitApplication(Guid applicationId);
    }
}