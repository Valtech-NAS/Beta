namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class RegisterCandidateStrategy : IRegisterCandidateStrategy
    {
        private readonly IRegistrationService _registrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        public RegisterCandidateStrategy(IRegistrationService registrationService,
            IAuthenticationService authenticationService, ICandidateWriteRepository candidateWriteRepository)
        {
            _registrationService = registrationService;
            _authenticationService = authenticationService;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            var username = newCandidate.RegistrationDetails.EmailAddress;
            var newCandidateId = Guid.NewGuid();
            var activationCode = "TODO"; //todo: generate a unique activation code (ICodeProvider)

            newCandidate.EntityId = newCandidateId;

            _registrationService.Register(username, newCandidateId, activationCode);

            _authenticationService.CreateUser(newCandidateId, password);

            var candidate = _candidateWriteRepository.Save(newCandidate);

            return candidate;
        }
    }
}
