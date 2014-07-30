namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;

    public class SendAccountUnlockCodeStrategy : ISendAccountUnlockCodeStrategy
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILockUserStrategy _lockUserStrategy;
        private readonly ICommunicationService _communicationService;

        public SendAccountUnlockCodeStrategy(
            IConfigurationManager configurationManager,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository,
            ILockUserStrategy lockUserStrategy,
            ICommunicationService communicationService)
        {
            _configurationManager = configurationManager;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _lockUserStrategy = lockUserStrategy;
            _communicationService = communicationService;
        }

        public void SendAccountUnlockCode(string username)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Cannot send unlock code if user is not locked.", UserStatuses.Locked);

            // TODO: AG: possible enhancement to have _candidateReadRepository.Get() throw on not found, as per _userReadRepository.
            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (candidate == null)
            {
                // TODO: AG: do not like to silently consume issues like 'candidate not found'.
                return;
            }

            if (user.AccountUnlockCodeExpiry < DateTime.Now)
            {
                // Account unlock code has expired, renew it.
                _lockUserStrategy.LockUser(user);
            }

            var unlockCodeExpiryDays = _configurationManager.GetAppSetting<int>("UnlockCodeExpiryDays");

            var firstName = candidate.RegistrationDetails.FirstName;
            var emailAddress = candidate.RegistrationDetails.EmailAddress;
            var accountUnlockCode = user.AccountUnlockCode;
            var expiryInDays = string.Format(unlockCodeExpiryDays == 1 ? "{0} day" : "{0} days", unlockCodeExpiryDays);

            var tokens = new[]
            {
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.CandidateFirstName, firstName),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.Username, emailAddress),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCode, accountUnlockCode),
                new KeyValuePair<CommunicationTokens, string>(CommunicationTokens.AccountUnlockCodeExpiryDays, expiryInDays)
            };

            _communicationService.SendMessageToCandidate(
                candidate.EntityId, CandidateMessageTypes.SendAccountUnlockCode, tokens);
        }
    }
}
