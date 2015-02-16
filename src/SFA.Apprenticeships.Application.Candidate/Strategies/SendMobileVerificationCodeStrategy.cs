namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Interfaces.Communications;

    public class SendMobileVerificationCodeStrategy : ISendMobileVerificationCodeStrategy
    {
        private readonly ICommunicationService _communicationService;

        public SendMobileVerificationCodeStrategy(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public void SendMobileVerificationCode(Candidate candidate)
        {
            var mobileNumber = candidate.RegistrationDetails.PhoneNumber;
            var mobileVerificationCode = candidate.CommunicationPreferences.MobileVerificationCode;

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendMobileVerificationCode,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateMobileNumber, mobileNumber),
                    new CommunicationToken(CommunicationTokens.MobileVerificationCode, mobileVerificationCode)
                });
        }
    }
}