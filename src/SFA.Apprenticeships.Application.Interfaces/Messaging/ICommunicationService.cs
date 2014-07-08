namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Responsible for gathering required info for sending SMS / Email messages to users. 
    /// Sending is delegated to a strategy for each message
    /// </summary>
    public interface ICommunicationService
    {
        void SendMessageToCandidate(Guid candidateId, CandidateMessageTypes messageType, IEnumerable<KeyValuePair<string, string>> tokens);
    }
}
