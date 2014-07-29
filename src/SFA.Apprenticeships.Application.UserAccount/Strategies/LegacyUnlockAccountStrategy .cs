namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using Candidate.Strategies;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class LegacyUnlockAccountStrategy : IUnlockAccountStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;
        private readonly IUserReadRepository _userReadRepository;

        public LegacyUnlockAccountStrategy(IUnlockAccountStrategy unlockAccountStrategy,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            ILegacyCandidateProvider legacyCandidateProvider,
            IUserReadRepository userReadRepository)
        {
            _unlockAccountStrategy = unlockAccountStrategy;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
            _userReadRepository = userReadRepository;
        }

        public void UnlockAccount(string username, string accountUnlockCode)
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

            user.AssertState("User should be a locked state", UserStatuses.Locked);

            if (candidate.LegacyCandidateId > 0)
            {
                _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
            }
            else
            {
                // Create candidate on legacy system and unlock account
                var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
                candidate.LegacyCandidateId = legacyCandidateId;
                _candidateWriteRepository.Save(candidate);
                _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
            }
        }
    }
}