namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used for sending SMS / Email messages to users. 
    /// Responsible for delegating request to message specific strategies. 
    /// Relevant data is collected in each message strategy and queued for later processing
    /// </summary>
    public interface ICommunicationService
    {
        void SendMessageToCandidate(Guid candidateId, MessageTypes messageType,
            IList<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}