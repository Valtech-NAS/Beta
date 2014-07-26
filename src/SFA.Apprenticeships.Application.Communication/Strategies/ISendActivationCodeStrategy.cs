namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Interfaces.Messaging;

    public interface ISendActivationCodeStrategy
    {
        void Send(Candidate candidate, CandidateMessageTypes messageType,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}