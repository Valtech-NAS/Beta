namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using UserAccount.Strategies;

    public class AuthenticateCandidateStrategy : IAuthenticateCandidateStrategy
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILockAccountStrategy _lockAccountStrategy;
        private readonly IConfigurationManager _configManager;

        public AuthenticateCandidateStrategy(
            IConfigurationManager configManager,
            IAuthenticationService authenticationService,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ILockAccountStrategy lockAccountStrategy)
        {
            _configManager = configManager;
            _userWriteRepository = userWriteRepository;
            _authenticationService = authenticationService;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _lockAccountStrategy = lockAccountStrategy;
        }

        public Candidate AuthenticateCandidate(string username, string password)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState(
                string.Format("Cannot authenticate user in state: {0}.", user.Status),
                UserStatuses.Active, UserStatuses.PendingActivation);

            var authenticated = _authenticationService.AuthenticateUser(user.EntityId, password);

            if (authenticated)
            {
                // TODO: AG: US458: user.ClearPasswordResetAttributes();
                var candidate = _candidateReadRepository.Get(user.EntityId);

                //_auditLog.Info(AuditEvents.SuccessfulLogon, username); // TODO: audit successful logon (named logger)

                return candidate;
            }

            RegisterFailedLogin(user);
            // either way, throw an exception to indicate failed auth

            // TODO: AG: US458: let's start creating some of these (or use AuthenticateResult).
            throw new Exception("Authentication failed"); // TODO: EXCEPTION: should use an application exception type
        }

        #region Helpers

        private void RegisterFailedLogin(User user)
        {
            // TODO: AG: need class for application setting names.
            var maximumPasswordAttemptsAllowed = _configManager.GetAppSetting<int>("MaximumPasswordAttemptsAllowed");

            if (user.LoginIncorrectAttempts == maximumPasswordAttemptsAllowed)
            {
                _lockAccountStrategy.LockAccount(user);
            }
            else
            {
                user.LoginIncorrectAttempts++;
                _userWriteRepository.Save(user);
            }
        }

        #endregion
    }
}
