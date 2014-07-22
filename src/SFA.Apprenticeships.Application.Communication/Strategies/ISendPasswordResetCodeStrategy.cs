namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface ISendPasswordResetCodeStrategy
    {
        void Send(string templateName, Candidate candidate, string passwordResetCode);
    }
}
