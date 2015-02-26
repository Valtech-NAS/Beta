namespace SFA.Apprenticeships.Domain.Entities
{
    using System.Collections.Generic;

    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string EmailContent { get; set; }
        public string Subject { get; set; }
    }
}