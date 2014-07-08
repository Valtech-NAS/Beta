namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used to send SMS / Email messages to users
    /// </summary>
    public interface ICommunicationService
    {
        void SendMessageToCandidate(Guid candidateId, string messageId, params KeyValuePair<string, string>[] tokens);
    }
}
