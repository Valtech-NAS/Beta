namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using Application.Interfaces.Messaging;

    public class VoidEmailDispatcher : IEmailDispatcher
    {
        public void SendEmail(EmailRequest request)
        {
            // We don't want to send any email.
        }
    }
}