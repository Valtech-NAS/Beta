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

    public class LockAccountStrategy : ILockAccountStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;
        private readonly int _unlockCodeExpiryDays;
        private readonly IUserWriteRepository _userWriteRepository;

        public LockAccountStrategy(IUserWriteRepository userWriteRepository,
            ICodeGenerator codeGenerator,
            ICommunicationService communicationService,
            IConfigurationManager configurationManager,
            ICandidateReadRepository candidateReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _codeGenerator = codeGenerator;
            _communicationService = communicationService;
            _candidateReadRepository = candidateReadRepository;
            _unlockCodeExpiryDays = configurationManager.GetAppSetting<int>("UnlockCodeExpiryDays");
        }

        public void LockAccount(User user)
        {
            if (user == null)
            {
                return;
            }

            //create and set an unlock code, set code expiry, save user, send email containing unlock code
            var accountUnlockCode = _codeGenerator.Generate();
            var currentDateTime = DateTime.Now;
            var expiry = currentDateTime.AddDays(_unlockCodeExpiryDays);

            user.SetStateLocked(accountUnlockCode, expiry);
            _userWriteRepository.Save(user);

            SendAccountUnlockCode(user, accountUnlockCode);
        }

        private void SendAccountUnlockCode(User user, string accountUnlockCode)
        {
            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate == null) { return; }

            var firstName = candidate.RegistrationDetails.FirstName;
            var emailAddress = candidate.RegistrationDetails.EmailAddress;
            var expiryInDays = string.Format(_unlockCodeExpiryDays == 1 ? "{0} day" : "{0} days", _unlockCodeExpiryDays);

            _communicationService.SendMessageToCandidate(candidate.EntityId, CandidateMessageTypes.SendAccountUnlockCode,
                new[]
                {
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, firstName),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, emailAddress),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCode, accountUnlockCode),
                    new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCodeExpiryDays,
                        expiryInDays)
                });
        }
    }
}