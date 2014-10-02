namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using Application.Interfaces.Messaging;

    internal class VoidEmailDispatcher : IEmailDispatcher
    {
        public void SendEmail(EmailRequest request)
        {
            // We don't want to send any email.
        }
    }
}