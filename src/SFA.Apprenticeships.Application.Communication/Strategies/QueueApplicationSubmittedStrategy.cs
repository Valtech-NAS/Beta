namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public class QueueApplicationSubmittedStrategy : ISendApplicationSubmittedStrategy
    {
        public void Send(string templateName, Candidate candidate, ApplicationDetail applicationDetail)
        {
            throw new NotImplementedException();
        }
    }
}
