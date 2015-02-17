namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Users;
    using ErrorCodes = Domain.Entities.ErrorCodes;

    public class SendMobileVerificationCodeStrategy : ISendMobileVerificationCodeStrategy
    {
        private readonly ICommunicationService _communicationService;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICodeGenerator _codeGenerator;

        public SendMobileVerificationCodeStrategy(ICommunicationService communicationService, ICandidateWriteRepository candidateWriteRepository, ICodeGenerator codeGenerator)
        {
            _communicationService = communicationService;
            _candidateWriteRepository = candidateWriteRepository;
            _codeGenerator = codeGenerator;
        }

        public void SendMobileVerificationCode(Candidate candidate)
        {
            if (!candidate.MobileVerificationRequired())
            {
                var message = string.Format("The mobile number associated with candidate Id: {0} does not require verification.", candidate.EntityId);
                throw new CustomException(message, ErrorCodes.EntityStateError);
            }

            if (string.IsNullOrEmpty(candidate.CommunicationPreferences.MobileVerificationCode))
            {
                candidate.CommunicationPreferences.MobileVerificationCode = _codeGenerator.GenerateNumeric();
                _candidateWriteRepository.Save(candidate);
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