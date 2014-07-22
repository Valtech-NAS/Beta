namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface ISendAccountUnlockCodeStrategy
    {
        void Send(string templateName, Candidate candidate, string accountUnlockCode);
    }
}
