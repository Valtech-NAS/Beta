namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailFromResolvers
{
    using Application.Interfaces.Communications;

    public interface IEmailFromResolver
    {
        bool CanResolve(MessageTypes messageType);

        string Resolve(EmailRequest emailRequest, string defaultEmailAddress);
    }
}