namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Interfaces.Messaging;

    public class QueueEmailOnlyAccountUnlockCodeStrategy : ISendAccountUnlockCodeStrategy
    {
        public void Send(Candidate candidate, CandidateMessageTypes messageType, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
