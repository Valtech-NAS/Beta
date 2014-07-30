namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface ISubmitApplicationStrategy
    {
        void SubmitApplication(Guid applicationId);
    }
}