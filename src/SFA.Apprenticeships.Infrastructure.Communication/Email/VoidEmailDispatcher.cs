namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using Application.Interfaces.Communications;

    public class VoidEmailDispatcher : IEmailDispatcher
    {
        public void SendEmail(EmailRequest request)
        {
            // We don't want to send any email.
        }
    }
}