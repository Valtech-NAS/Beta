namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Interfaces.Users;

    public class SendPasswordResetCodeStrategy : ISendPasswordResetCodeStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;
        private readonly int _passwordResetCodeExpiryDays;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public SendPasswordResetCodeStrategy(IConfigurationManager configurationManager,
            ICommunicationService communicationService, ICodeGenerator codeGenerator,
            IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository,
            ICandidateReadRepository candidateReadRepository)
        {
            _communicationService = communicationService;
            _codeGenerator = codeGenerator;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _passwordResetCodeExpiryDays = configurationManager.GetAppSetting<int>("PasswordResetCodeExpiryDays");
        }

        public void SendPasswordResetCode(string username)
        {
            var user = _userReadRepository.Get(username, false);

            if (user == null)
            {
                throw new Exception("Unknown user name");
            }

            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate == null)
            {
                throw new Exception("Unknown candidate");
            }

            var currentDateTime = DateTime.Now;
            var expiry = currentDateTime.AddDays(_passwordResetCodeExpiryDays);

            if (!string.IsNullOrEmpty(user.PasswordResetCode) && (user.PasswordResetCodeExpiry > currentDateTime))
            {
                // Reuse existing token and set new expiry date
                user.PasswordResetCodeExpiry = expiry;
            }
            else
            {
                // generate new code and send
                var passwordResetCode = _codeGenerator.Generate();
                user.SetStatePasswordResetCode(passwordResetCode, expiry);
            }

            _userWriteRepository.Save(user);

            // Send Password Reset Code
            SendPasswordResetCodeViaCommunicationService(candidate, user.PasswordResetCode);
        }

        private void SendPasswordResetCodeViaCommunicationService(Candidate candidate, string passwordResetCode)
        {
            if (candidate == null)
            {
                return;
            }

            var firstName = candidate.RegistrationDetails.FirstName;
            var emailAddress = candidate.RegistrationDetails.EmailAddress;
            var expiry = string.Format(_passwordResetCodeExpiryDays == 1 ? "{0} day" : "{0} days", _passwordResetCodeExpiryDays);

            _communicationService.SendMessageToCandidate(candidate.EntityId, CandidateMessageTypes.SendPasswordResetCode,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, firstName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, emailAddress),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.PasswordResetCode, passwordResetCode),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.PasswordResetCodeExpiryDays, expiry)
                });
        }
    }
}