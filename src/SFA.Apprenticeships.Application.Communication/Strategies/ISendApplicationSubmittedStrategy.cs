namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public interface ISendApplicationSubmittedStrategy
    {
        void Send(string templateName, Candidate candidate, ApplicationDetail applicationDetail);
    }
}
