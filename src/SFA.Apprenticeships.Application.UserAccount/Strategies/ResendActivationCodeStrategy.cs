namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Interfaces.Users;

    public class ResendActivationCodeStrategy : IResendActivationCodeStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly int _activationCodeExpiryDays;

        public ResendActivationCodeStrategy(IConfigurationManager configurationManager,ICommunicationService communicationService,
            ICandidateReadRepository candidateReadRepository, ICodeGenerator codeGenerator,
            IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            _communicationService = communicationService;
            _candidateReadRepository = candidateReadRepository;
            _codeGenerator = codeGenerator;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _activationCodeExpiryDays = configurationManager.GetAppSetting<int>("ActivationCodeExpiryDays");
        }

        public void ResendActivationCode(string username)
        {
            var user = _userReadRepository.Get(username, false);

            if (user == null)
            {
                // TODO: why not just allow _userReadRepository to throw?
                throw new CustomException("Unknown username", ErrorCodes.UnknownUserError);
            }

            user.AssertState("Username already in use and is not in pending activation status", UserStatuses.PendingActivation);

            var candidate = _candidateReadRepository.Get(user.EntityId);

            var currentDateTime = DateTime.Now;
            var expiry = currentDateTime.AddDays(_activationCodeExpiryDays);

            if (!string.IsNullOrEmpty(user.ActivationCode) && (user.ActivateCodeExpiry > currentDateTime))
            {
                // Reuse existing token and set new expiry date
                user.PasswordResetCodeExpiry = expiry;
            }
            else
            {
                // generate new code and send
                var activationCode = _codeGenerator.Generate();
                user.SetStatePendingActivation(activationCode, expiry);
            }

            _userWriteRepository.Save(user);

           SendActivationCode(candidate, user.ActivationCode);
        }

        private void SendActivationCode(Candidate candidate, string activationCode)
        {
            var emailAddress = candidate.RegistrationDetails.EmailAddress;
            var expiry = FormatActivationCodeExpiryDays();
            var firstName = candidate.RegistrationDetails.FirstName;

            _communicationService.SendMessageToCandidate(candidate.EntityId, CandidateMessageTypes.SendActivationCode,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, firstName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCode, activationCode),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.ActivationCodeExpiryDays, expiry),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, emailAddress)
                });
        }

        private string FormatActivationCodeExpiryDays()
        {
            return string.Format(_activationCodeExpiryDays == 1 ? "{0} day" : "{0} days", _activationCodeExpiryDays);
        }
    }
}