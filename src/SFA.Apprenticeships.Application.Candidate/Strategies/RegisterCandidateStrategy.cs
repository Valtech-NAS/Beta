namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Interfaces.Users;

    public class RegisterCandidateStrategy : IRegisterCandidateStrategy
    {
        private readonly IRegistrationService _registrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ICodeGenerator _codeGenerator;

        public RegisterCandidateStrategy(IRegistrationService registrationService, IAuthenticationService authenticationService, ICandidateWriteRepository candidateWriteRepository, ICommunicationService communicationService, ICodeGenerator codeGenerator)
        {
            _registrationService = registrationService;
            _authenticationService = authenticationService;
            _candidateWriteRepository = candidateWriteRepository;
            _communicationService = communicationService;
            _codeGenerator = codeGenerator;
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            var username = newCandidate.RegistrationDetails.EmailAddress;
            var newCandidateId = Guid.NewGuid();
            var activationCode = _codeGenerator.Generate();

            newCandidate.EntityId = newCandidateId;

            _registrationService.Register(username, newCandidateId, activationCode, UserRoles.Candidate);

            _authenticationService.CreateUser(newCandidateId, password);

            var candidate = _candidateWriteRepository.Save(newCandidate);

            _communicationService.SendMessageToCandidate(newCandidateId, CandidateMessageTypes.SendActivationCode,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateLastName, candidate.RegistrationDetails.LastName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCode, activationCode)
                });

            return candidate;
        }
    }
}
