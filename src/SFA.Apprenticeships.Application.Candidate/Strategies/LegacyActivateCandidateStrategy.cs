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
            //var user = _userReadRepository.Get(username);
            //var candidate = _candidateReadRepository.Get(user.EntityId);

            //todo: assert status
            //user.Status == UserStatuses.PendingActivation

            //todo: validate activation code and update user status to "activated"
            //_registrationService.Activate(username, activationCode);

            //todo: create candidate in legacy
            //var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);

            //todo: update candidate with legacy candidate id
            //candidate.LegacyCandidateId = legacyCandidateId;
            //_candidateWriteRepository.Save(candidate);
        }
    }
}
