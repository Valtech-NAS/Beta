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
            User user = _userReadRepository.Get(username);

            if (user == null)
            {
                throw new Exception("Unknown user name");
            }

            Candidate candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate == null)
            {
                throw new Exception("Unknown candidate");
            }

            DateTime currentDateTime = DateTime.Now;
            DateTime expiry = currentDateTime.AddDays(_passwordResetCodeExpiryDays);

            if (!string.IsNullOrEmpty(user.PasswordResetCode) && (user.PasswordResetCodeExpiry > currentDateTime))
            {
                // Reuse existing token and set new expiry date
                user.PasswordResetCodeExpiry = expiry;
            }
            else
            {
                // generate new code and send
                string passwordResetCode = _codeGenerator.Generate();
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

            string firstName = candidate.RegistrationDetails.FirstName;
            string emailAddress = candidate.RegistrationDetails.EmailAddress;
            string expiry = string.Format("{0}", _passwordResetCodeExpiryDays);

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