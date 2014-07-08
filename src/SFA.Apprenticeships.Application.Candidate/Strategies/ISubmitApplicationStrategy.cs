namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public interface ISubmitApplicationStrategy
    {
        void SubmitApplication(ApplicationDetail application);
    }
}
