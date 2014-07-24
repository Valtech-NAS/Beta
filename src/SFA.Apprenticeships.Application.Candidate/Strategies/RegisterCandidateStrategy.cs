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
        private readonly IUserAccountService _registrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ICodeGenerator _codeGenerator;

        public RegisterCandidateStrategy(IUserAccountService registrationService,
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
            var username = newCandidate.RegistrationDetails.EmailAddress;
            var user = _userReadRepository.Get(username, false);

            if (user != null && user.ActivateCodeExpiry.HasValue && user.ActivateCodeExpiry < DateTime.Today)
            {
                //user activation has expired - this is just to get existing code working
            }

            // todo: if username in use and pending and not expired activation code then need to reuse the existing user
            // todo: if username in use and pending and expired activation code then need to overwrite the existing one (incl p/w in AD)

            var newCandidateId = Guid.NewGuid();

            _authenticationService.CreateUser(newCandidateId, password);

            newCandidate.EntityId = newCandidateId;

            var activationCode = _codeGenerator.Generate();

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
