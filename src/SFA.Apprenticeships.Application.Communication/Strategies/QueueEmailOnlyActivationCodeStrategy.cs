namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public class QueueEmailOnlyActivationCodeStrategy : ISendActivationCodeStrategy
    {
        public void Send(Candidate candidate, string activationCode)
        {
            //todo: create an EmailRequest to the outbound communications queue... will then be picked up and sent to IEmailDispatcher.SendEmail()
            throw new NotImplementedException();
        }
    }
}
