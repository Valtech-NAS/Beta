namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;

    public class SubmitContactMessageStrategy : ISubmitContactMessageStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationManager _configurationManager;

        public SubmitContactMessageStrategy(ICommunicationService communicationService, IConfigurationManager configurationManager)
        {
            _communicationService = communicationService;
            _configurationManager = configurationManager;
        }

        public void SubmitMessage(ContactMessage message)
        {
            var helpdeskAddress = _configurationManager.GetAppSetting<string>("HelpdeskEmailAddress");

            _communicationService.SendContactMessage(message.UserId, MessageTypes.CandidateContactMessage, new[]
            {
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, helpdeskAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, message.Email),
                new CommunicationToken(CommunicationTokens.UserFullName, message.Name),
                new CommunicationToken(CommunicationTokens.UserEnquiry, message.Enquiry),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, message.Details)
            });
        }
    }
}
