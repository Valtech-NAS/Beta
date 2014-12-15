namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using UserAccount.Strategies;

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
            var user = _userReadRepository.Get(username);         

            var candidate = _candidateReadRepository.Get(user.EntityId);

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