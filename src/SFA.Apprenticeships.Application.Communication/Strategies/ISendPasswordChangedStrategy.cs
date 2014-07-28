namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Interfaces.Messaging;

    public interface ISendPasswordChangedStrategy
    {
        void Send(Candidate candidate, CandidateMessageTypes messageTypes,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}