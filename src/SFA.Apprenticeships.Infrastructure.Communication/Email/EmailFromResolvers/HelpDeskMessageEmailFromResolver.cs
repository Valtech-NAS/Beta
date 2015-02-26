namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailFromResolvers
{
    using System.Linq;
    using Application.Interfaces.Communications;

    public class HelpDeskMessageEmailFromResolver : IEmailFromResolver
    {
        public bool CanResolve(MessageTypes messageType)
        {
            return messageType == MessageTypes.CandidateContactMessage;
        }

        public string Resolve(EmailRequest emailRequest, string defaultEmailAddress)
        {
            return emailRequest.Tokens.First(t => t.Key == CommunicationTokens.UserEmailAddress).Value;
        }
    }
}