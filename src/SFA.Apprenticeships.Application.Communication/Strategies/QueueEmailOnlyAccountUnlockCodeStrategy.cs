namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public class QueueEmailOnlyAccountUnlockCodeStrategy : ISendAccountUnlockCodeStrategy
    {
        public void Send(string templateName, Candidate candidate, string accountUnlockCode)
        {
            throw new NotImplementedException();
        }
    }
}
