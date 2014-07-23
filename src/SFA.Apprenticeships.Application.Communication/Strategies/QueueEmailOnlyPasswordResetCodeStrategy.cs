namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public class QueueEmailOnlyPasswordResetCodeStrategy : ISendPasswordResetCodeStrategy
    {
        public void Send(string templateName, Candidate candidate, string passwordResetCode)
        {
            throw new NotImplementedException();
        }
    }
}
