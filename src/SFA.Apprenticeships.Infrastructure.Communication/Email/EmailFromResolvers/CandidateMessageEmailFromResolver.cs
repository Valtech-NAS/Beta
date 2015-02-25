namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailFromResolvers
{
    using Application.Interfaces.Communications;

    public class CandidateMessageEmailFromResolver : IEmailFromResolver
    {
        public bool CanResolve(MessageTypes messageType)
        {
            return messageType != MessageTypes.CandidateContactMessage;
        }

        public string Resolve(EmailRequest emailRequest, string defaultEmailAddress)
        {
            return defaultEmailAddress;
        }
    }
}