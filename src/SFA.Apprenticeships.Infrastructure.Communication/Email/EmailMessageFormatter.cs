namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using Application.Interfaces.Communications;
    using SendGrid;

    public abstract class EmailMessageFormatter
    {
        public abstract void PopulateMessage(EmailRequest request, SendGridMessage message);
    }
}