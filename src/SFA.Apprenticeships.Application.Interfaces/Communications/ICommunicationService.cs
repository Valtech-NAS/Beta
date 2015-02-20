namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used for sending SMS / Email messages to users. 
    /// Responsible for delegating request to message specific strategies. 
    /// Relevant data is collected by the caller
    /// </summary>
    public interface ICommunicationService
    {
        void SendMessageToCandidate(Guid candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);

        void SendContactMessage(Guid? userId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
