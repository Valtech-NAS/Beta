
namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;

    public class VerifyMobileStrategy : IVerifyMobileStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private ICandidateWriteRepository _candidateWriteRepository;

        public VerifyMobileStrategy(ICandidateReadRepository candidateReadRepository, ICandidateWriteRepository candidateWriteRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public void VerifyMobile(Guid candidateId, string verificationCode)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            if (!candidate.MobileVerificationRequired())
            {
                var message = string.Format("The mobile number associated with candidate Id: {0} does not require verification.", candidate.EntityId);
                throw new CustomException(message, Domain.Entities.ErrorCodes.EntityStateError);
            }


            if (candidate.CommunicationPreferences.MobileVerificationCode == verificationCode)
            {
                candidate.CommunicationPreferences.MobileVerificationCode = string.Empty;
                candidate.CommunicationPreferences.VerifiedMobile = true;
                _candidateWriteRepository.Save(candidate);
            }
            else
            {
                var errorMessage =string.Format("Mobile verification code {0} is invalid for candidate {1} with mobile number {2}", verificationCode, candidateId, candidate.RegistrationDetails.PhoneNumber);
                throw new CustomException(errorMessage, Interfaces.Users.ErrorCodes.MobileCodeVerificationFailed);
            }
        }
    }
}
