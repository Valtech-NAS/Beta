namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface ISubmitApprenticeshipApplicationStrategy
    {
        void SubmitApplication(Guid applicationId);
    }
}