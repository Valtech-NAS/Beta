namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DTO to represent an SMS that should be sent
    /// </summary>
    public class SmsRequest
    {
        public string Sender { get; set; } //todo: remove as (will be) defined in config

        public string ToNumber { get; set; }

        public string Message { get; set; }
        public MessageTypes MessageType { get; set; }

        public IEnumerable<KeyValuePair<CommunicationTokens, string>> Tokens { get; set; }
    }
}
