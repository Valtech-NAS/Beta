namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class LegacyActivateCandidateStrategy : IActivateCandidateStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserAccountService _registrationService;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public LegacyActivateCandidateStrategy(IUserReadRepository userReadRepository, IUserAccountService registrationService, ILegacyCandidateProvider legacyCandidateProvider, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository)
        {
            _userReadRepository = userReadRepository;
            _registrationService = registrationService;
            _legacyCandidateProvider = legacyCandidateProvider;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public void ActivateCandidate(string username, string activationCode)
        {
            var user = _userReadRepository.Get(username, false);

            if (user == null)
            {
                throw new CustomException("Unknown username", ErrorCodes.UnknownUserError);
            }

            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate == null)
            {
                throw new CustomException("Unknown Candidate", ErrorCodes.UnknownCandidateError);
            }

            user.AssertState("User is in invalid state for activation", UserStatuses.PendingActivation);

            if (candidate.LegacyCandidateId > 0)
            {
                _registrationService.Activate(username, activationCode);
            }
            else
            {
                // Create candidate on legacy system before activating
                var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
                candidate.LegacyCandidateId = legacyCandidateId;
                _candidateWriteRepository.Save(candidate);
                _registrationService.Activate(username, activationCode);
            }
        }
    }
}
