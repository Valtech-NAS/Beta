namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public class QueueEmailOnlyPasswordChangedStrategy : ISendPasswordChangedStrategy
    {
        public void Send(string templateName, Candidate candidate)
        {
            throw new NotImplementedException();
        }
    }
}
