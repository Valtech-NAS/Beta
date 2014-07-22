namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface ISendPasswordChangedStrategy
    {
        void Send(string templateName, Candidate candidate);
    }
}
