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
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ICodeGenerator _codeGenerator;

        public RegisterCandidateStrategy(IRegistrationService registrationService,
            IAuthenticationService authenticationService, ICandidateWriteRepository candidateWriteRepository,
            ICommunicationService communicationService, ICodeGenerator codeGenerator,
            IUserReadRepository userReadRepository)
        {
            _registrationService = registrationService;
            _authenticationService = authenticationService;
            _candidateWriteRepository = candidateWriteRepository;
            _communicationService = communicationService;
            _codeGenerator = codeGenerator;
            _userReadRepository = userReadRepository;
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            var newCandidateId = Guid.NewGuid();

            _authenticationService.CreateUser(newCandidateId, password);

            newCandidate.EntityId = newCandidateId;

            var activationCode = _codeGenerator.Generate();

            var username = newCandidate.RegistrationDetails.EmailAddress;

            //todo: check if username is already in use
            var user = _userReadRepository.Get(username);
            // todo: if username in use and pending and not expired activation code then need to reuse the existing user
            // todo: if username in use and pending but has expired activation code then need to overwrite the existing one (incl p/w)

            _registrationService.Register(username, newCandidateId, activationCode, UserRoles.Candidate);

            var candidate = _candidateWriteRepository.Save(newCandidate);

            SendActivationCode(candidate.EntityId, candidate, activationCode);

            return candidate;
        }

        #region Helpers

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

        #endregion
    }
}
