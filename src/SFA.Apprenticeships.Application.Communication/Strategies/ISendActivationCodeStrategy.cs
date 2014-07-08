namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface ISendActivationCodeStrategy
    {
        void Send(Candidate candidate, string activationCode);
    }
}
