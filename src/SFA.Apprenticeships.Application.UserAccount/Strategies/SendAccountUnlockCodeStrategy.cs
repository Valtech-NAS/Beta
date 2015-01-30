namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;

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

            user.AssertState("Send unlock code", UserStatuses.Locked);

            var candidate = _candidateReadRepository.Get(user.EntityId);

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
                new CommunicationToken(CommunicationTokens.CandidateFirstName, firstName),
                new CommunicationToken(CommunicationTokens.Username, emailAddress),
                new CommunicationToken(CommunicationTokens.AccountUnlockCode, accountUnlockCode),
                new CommunicationToken(CommunicationTokens.AccountUnlockCodeExpiryDays, expiryInDays)
            };

            _communicationService.SendMessageToCandidate(
                candidate.EntityId, MessageTypes.SendAccountUnlockCode, tokens);
        }
    }
}
