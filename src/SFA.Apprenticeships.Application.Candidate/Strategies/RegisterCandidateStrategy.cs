namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Interfaces.Users;

    public class RegisterCandidateStrategy : IRegisterCandidateStrategy
    {
        private readonly int _activationCodeExpiryDays;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;
        private readonly IUserAccountService _registrationService;
        private readonly IUserReadRepository _userReadRepository;

        public RegisterCandidateStrategy(IConfigurationManager configurationManager,
            IUserAccountService registrationService,
            IAuthenticationService authenticationService,
            ICandidateWriteRepository candidateWriteRepository,
            ICommunicationService communicationService,
            ICodeGenerator codeGenerator,
            IUserReadRepository userReadRepository)
        {
            _registrationService = registrationService;
            _authenticationService = authenticationService;
            _candidateWriteRepository = candidateWriteRepository;
            _communicationService = communicationService;
            _codeGenerator = codeGenerator;
            _userReadRepository = userReadRepository;
            _activationCodeExpiryDays = configurationManager.GetAppSetting<int>("ActivationCodeExpiryDays");
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            string username = newCandidate.RegistrationDetails.EmailAddress;
            string activationCode = _codeGenerator.Generate();
            User user = _userReadRepository.Get(username, false);


            if (user == null)
            {
                // Process registration for brand new username
                Guid newCandidateId = Guid.NewGuid();
                _authenticationService.CreateUser(newCandidateId, password);
                _registrationService.Register(username, newCandidateId, activationCode, UserRoles.Candidate);
                return SaveAndNotifyCandidate(newCandidateId, newCandidate, activationCode);
            }

            if (user.Status != UserStatuses.PendingActivation)
            {
                throw new Exception("Username registered and not in pending activation state");
                //TODO Customer exception
            }

            if (user.ActivateCodeExpiry > DateTime.Now)
            {
                // Process existing username in unexpired pending activation status
                return SaveAndNotifyCandidate(user.EntityId, newCandidate, user.ActivationCode);
            }

            // Process existing username in an expired pending activation status
            _authenticationService.ResetUserPassword(user.EntityId, password);
            _registrationService.Register(username, user.EntityId, user.ActivationCode, UserRoles.Candidate);
            return SaveAndNotifyCandidate(user.EntityId, newCandidate, activationCode);
        }

        #region Helpers

        private Candidate SaveAndNotifyCandidate(Guid candidateId, Candidate newCandidate, string activationCode)
        {
            newCandidate.EntityId = candidateId;
            Candidate candidate = _candidateWriteRepository.Save(newCandidate);
            SendActivationCode(candidate, activationCode);
            return candidate;
        }

        private void SendActivationCode(Candidate candidate, string activationCode)
        {
            if (candidate == null)
            {
                return;
            }

            string emailAddress = candidate.RegistrationDetails.EmailAddress;
            string expiry = string.Format("{0}", _activationCodeExpiryDays);
            string firstName = candidate.RegistrationDetails.FirstName;

            _communicationService.SendMessageToCandidate(candidate.EntityId, CandidateMessageTypes.SendActivationCode,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, firstName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCode, activationCode),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCodeExpiryDays, expiry),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, emailAddress)
                });
        }

        #endregion
    }
}