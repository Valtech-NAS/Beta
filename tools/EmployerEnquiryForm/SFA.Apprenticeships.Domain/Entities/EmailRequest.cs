namespace SFA.Apprenticeships.Domain.Entities
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string EmailContent { get; set; }
        public string Subject { get; set; }
    }
}