namespace SFA.Apprenticeships.Application.UserAccount
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Users;
    using Strategies;
    using Web.Common.Constants;
    
    public class UserAccountService : IUserAccountService
    {
        private readonly ILogService _logger;

        private readonly IActivateUserStrategy _activateUserStrategy;
        private readonly IRegisterUserStrategy _registerUserStrategy;
        private readonly ISendAccountUnlockCodeStrategy _resendAccountUnlockCodeStrategy;
        private readonly IResendActivationCodeStrategy _resendActivationCodeStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISendPasswordResetCodeStrategy _sendPasswordCodeStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;
        private readonly IUserReadRepository _userReadRepository;

        public UserAccountService(IUserReadRepository userReadRepository,
            IRegisterUserStrategy registerUserStrategy,
            IActivateUserStrategy activateUserStrategy,
            IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            ISendPasswordResetCodeStrategy sendPasswordCodeStrategy,
            IResendActivationCodeStrategy resendActivationCodeStrategy,
            ISendAccountUnlockCodeStrategy resendAccountUnlockCodeStrategy,
            IUnlockAccountStrategy unlockAccountStrategy, ILogService logger)
        {
            _userReadRepository = userReadRepository;
            _registerUserStrategy = registerUserStrategy;
            _activateUserStrategy = activateUserStrategy;
            _resetForgottenPasswordStrategy = resetForgottenPasswordStrategy;
            _sendPasswordCodeStrategy = sendPasswordCodeStrategy;
            _resendActivationCodeStrategy = resendActivationCodeStrategy;
            _resendAccountUnlockCodeStrategy = resendAccountUnlockCodeStrategy;
            _unlockAccountStrategy = unlockAccountStrategy;
            _logger = logger;
        }

        public bool IsUsernameAvailable(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to discover if the username {0} is available.", username);

            // check status of user (unactivated account should also be considered "available")
            var user = _userReadRepository.Get(username, false);
            return user == null || user.Status == UserStatuses.PendingActivation;
        }

        public void Register(string username, Guid userId, string activationCode, UserRoles roles)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to register the user {0}.", username);

            _registerUserStrategy.Register(username, userId, activationCode, roles);
        }

        public void Activate(string username, string activationCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _logger.Info("Calling ActivateUserStrategy to activate the user {0}.", username);

            _activateUserStrategy.Activate(username, activationCode);

            _logger.Info("ActivateUserStrategy activated the user {0}.", username);
        }

        public void ResendActivationCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to resend the activation code for the user {0}.", username);

            _resendActivationCodeStrategy.ResendActivationCode(username);
        }

        public void SendPasswordResetCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to send the password reset code for the user {0}.", username);

            _sendPasswordCodeStrategy.SendPasswordResetCode(username);
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(passwordCode).IsNotNullOrEmpty();
            Condition.Requires(newPassword).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to reset the forgotten password for the user {0}.", username);

            _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
        }

        public void ResendAccountUnlockCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to resend the account unlock code for the user {0}.", username);

            _resendAccountUnlockCodeStrategy.SendAccountUnlockCode(username);
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(accountUnlockCode).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to unlock the account for the user {0}.", username);

            _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
        }

        public UserStatuses GetUserStatus(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to get the status for the user {0}.", username);

            var user = _userReadRepository.Get(username, false);

            return user == null ? UserStatuses.Unknown : user.Status;
        }

        public string[] GetRoleNames(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logger.Debug("Calling UserAccountService to get the role names for the user {0}.", username);

            var claims = new List<string>();
            var userStatus = GetUserStatus(username);

            // Add 'roles' for user status.
            switch (userStatus)
            {
                case UserStatuses.Active:
                    claims.Add(UserRoleNames.Activated);
                    break;

                case UserStatuses.PendingActivation:
                    claims.Add(UserRoleNames.Unactivated);
                    break;
            }

            // TODO: Add actual user roles.

            return claims.ToArray();
        }

        public void UpdateUserProfile(string username, RegistrationDetails profileDetails)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(profileDetails);

            //todo: allow update of name, DOB, address, contact number (not email address as new addresses must be confirmed)

            throw new NotImplementedException();
        }
    }
}
