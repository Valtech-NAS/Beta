namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Interfaces.Repositories;
    using UserAccount.Strategies;

    public class LegacyResetForgottenPasswordStrategy : IResetForgottenPasswordStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly IUserReadRepository _userReadRepository;

        public LegacyResetForgottenPasswordStrategy(IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            ILegacyCandidateProvider legacyCandidateProvider,
            IUserReadRepository userReadRepository)
        {
            _resetForgottenPasswordStrategy = resetForgottenPasswordStrategy;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
            _userReadRepository = userReadRepository;
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            var user = _userReadRepository.Get(username);        

            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate.LegacyCandidateId > 0)
            {
                _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
            }
            else
            {
                // Create candidate on legacy system and reset forgotten password
                var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
                candidate.LegacyCandidateId = legacyCandidateId;
                _candidateWriteRepository.Save(candidate);
                _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
            }
        }
    }
}