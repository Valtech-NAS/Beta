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
            var newCandidateId = Guid.NewGuid();

            //Create user in active directory first so that if that fails the user can try again with the same username
            CreateUserOnAuthenticationService(password, newCandidateId);

            newCandidate.EntityId = newCandidateId;

            //Generate activation code
            var activationCode = _codeGenerator.Generate();

            var username = newCandidate.RegistrationDetails.EmailAddress;

            //Create user on registration service
            CreateUserOnRegistrationService(username, newCandidate.EntityId, activationCode, UserRoles.Candidate);

            var candidate = _candidateWriteRepository.Save(newCandidate);

            //Send activation code
            SendActivationCode(candidate.EntityId, candidate, activationCode);

            return candidate;
        }

        #region Helpers
        
        private void CreateUserOnRegistrationService(string username, Guid newCandidateId, string activationCode, UserRoles role)
        {
            _registrationService.Register(username, newCandidateId, activationCode, role);
        }

        private void SendActivationCode(Guid newCandidateId, Candidate candidate, string activationCode)
        {
            _communicationService.SendMessageToCandidate(newCandidateId, CandidateMessageTypes.SendActivationCode,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName,
                        candidate.RegistrationDetails.FirstName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateLastName,
                        candidate.RegistrationDetails.LastName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCode, activationCode)
                });
        }

        private void CreateUserOnAuthenticationService(string password, Guid newCandidateId)
        {
            _authenticationService.CreateUser(newCandidateId, password);
        }

        #endregion
    }
}
