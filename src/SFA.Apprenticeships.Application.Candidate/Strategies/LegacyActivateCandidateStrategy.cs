namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class LegacyActivateCandidateStrategy : IActivateCandidateStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IRegistrationService _registrationService;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public LegacyActivateCandidateStrategy(IUserReadRepository userReadRepository, IRegistrationService registrationService, ILegacyCandidateProvider legacyCandidateProvider, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository)
        {
            _userReadRepository = userReadRepository;
            _registrationService = registrationService;
            _legacyCandidateProvider = legacyCandidateProvider;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
        }

        public void ActivateCandidate(string username, string activationCode)
        {
            var user = _userReadRepository.Get(username);
            var candidate = _candidateReadRepository.Get(user.EntityId);

            //todo: use a helper here to assert state == pending (no need to cover all other specific states)
            //user.AssertState(UserStatuses.PendingActivation, "User is in invalid state for activation");

            _registrationService.Activate(username, activationCode);
            
            var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
            candidate.LegacyCandidateId = legacyCandidateId;

            _candidateWriteRepository.Save(candidate);
        }
    }
}
