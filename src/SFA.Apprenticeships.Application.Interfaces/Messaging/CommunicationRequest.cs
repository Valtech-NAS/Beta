namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DTO to represent an outbound communication message (email, SMS)
    /// </summary>
    public class CommunicationRequest
    {
        public Guid EntityId { get; set; }

        public MessageTypes MessageType { get; set; }

        public IEnumerable<KeyValuePair<CommunicationTokens, string>> Tokens { get; set; }
    }
}
