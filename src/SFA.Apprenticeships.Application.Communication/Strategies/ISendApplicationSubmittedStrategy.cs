namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Interfaces.Messaging;

    public interface ISendApplicationSubmittedStrategy
    {
        void Send(Candidate candidate, ApplicationDetail applicationDetail, CandidateMessageTypes messageType,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}