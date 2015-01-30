namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DTO to represent an email that should be sent
    /// </summary>
    public class EmailRequest
    {
        public string ToEmail { get; set; }

        public MessageTypes MessageType { get; set; }

        public IEnumerable<CommunicationToken> Tokens { get; set; }
    }
}
