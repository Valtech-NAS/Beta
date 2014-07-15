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
        public string TemplateName { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Tokens { get; set; }
    }
}
