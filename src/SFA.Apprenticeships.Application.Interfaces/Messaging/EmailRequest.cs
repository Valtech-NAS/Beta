namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DTO to represent an email that should be sent
    /// </summary>
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public CandidateMessageTypes MessageType { get; set; }
        public IEnumerable<KeyValuePair<CommunicationTokens, string>> Tokens { get; set; }
    }
}
