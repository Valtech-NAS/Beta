namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using Domain.Entities.Users;
    using Domain.Entities.Candidates;

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

            switch (user.Status)
            {
                case UserStatuses.Unknown:
                    throw new Exception("Unknown user name"); //todo: should use an application exception type
                case UserStatuses.PendingActivation:
                    ProcessPendingActivation(username, activationCode, candidate);
                    break;
                case UserStatuses.Active:
                    throw new Exception("User is already active"); //todo: should use an application exception type
                case UserStatuses.Inactive:
                    throw new Exception("User is inactive"); //todo: should use an application exception type
                case UserStatuses.Blocked:
                    throw new Exception("User is blocked"); //todo: should use an application exception type
                default:
                    throw new Exception("User status is unknown"); //todo: should use an application exception type
            }
        }

        private void ProcessPendingActivation(string username, string activationCode, Candidate candidate)
        {
            _registrationService.Activate(username, activationCode);
            var legacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);
            candidate.LegacyCandidateId = legacyCandidateId;
            _candidateWriteRepository.Save(candidate);

            //todo: send a welcome email?
        }
    }
}
