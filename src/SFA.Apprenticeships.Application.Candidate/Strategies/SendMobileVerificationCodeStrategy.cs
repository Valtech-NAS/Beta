namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
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
            if (!candidate.MobileVerificationRequired())
            {
                var message = string.Format("The mobile number associated with candidate Id: {0} does not require verification.", candidate.EntityId);
                throw new CustomException(message, Domain.Entities.ErrorCodes.EntityStateError);
            }

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