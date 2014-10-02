namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System.Collections.Generic;

    public class LogRequest
    {
        public string LogMessage { get; set; }
        public IEnumerable<KeyValuePair<CommunicationTokens, string>> Tokens { get; set; }
        public CandidateMessageTypes MessageType { get; set; }
    }
}