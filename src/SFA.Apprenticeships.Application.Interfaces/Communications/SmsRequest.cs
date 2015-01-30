namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DTO to represent an SMS that should be sent
    /// </summary>
    public class SmsRequest
    {
        public string ToNumber { get; set; }

        public MessageTypes MessageType { get; set; }

        public IEnumerable<CommunicationToken> Tokens { get; set; }
    }
}
