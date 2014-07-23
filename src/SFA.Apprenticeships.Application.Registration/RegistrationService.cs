namespace SFA.Apprenticeships.Application.Registration
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using Strategies;

    public class RegistrationService : IRegistrationService
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IRegisterUserStrategy _registerUserStrategy;
        private readonly IActivateUserStrategy _activateUserStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISendPasswordResetCodeStrategy _sendPasswordCodeStrategy;
        private readonly IResendActivationCodeStrategy _resendActivationCodeStrategy;
        private readonly IResendAccountUnlockCodeStrategy _resendAccountUnlockCodeStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;

        public RegistrationService(IUserReadRepository userReadRepository,
            IRegisterUserStrategy registerUserStrategy,
            IActivateUserStrategy activateUserStrategy,
            IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            ISendPasswordResetCodeStrategy sendPasswordCodeStrategy,
            IResendActivationCodeStrategy resendActivationCodeStrategy,
            IResendAccountUnlockCodeStrategy resendAccountUnlockCodeStrategy,
            IUnlockAccountStrategy unlockAccountStrategy)
        {
            _userReadRepository = userReadRepository;
            _registerUserStrategy = registerUserStrategy;
            _activateUserStrategy = activateUserStrategy;
            _resetForgottenPasswordStrategy = resetForgottenPasswordStrategy;
            _sendPasswordCodeStrategy = sendPasswordCodeStrategy;
            _resendActivationCodeStrategy = resendActivationCodeStrategy;
            _resendAccountUnlockCodeStrategy = resendAccountUnlockCodeStrategy;
            _unlockAccountStrategy = unlockAccountStrategy;
        }

        public bool IsUsernameAvailable(string username)
        {
            // TODO: NOTIMPL: check status of user (unactivated account should also be considered "available")
            return _userReadRepository.Get(username, false) == null;
        }

        public void Register(string username, Guid userId, string activationCode, UserRoles roles)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _registerUserStrategy.Register(username, userId, activationCode, roles);
        }

        public void Activate(string username, string activationCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _activateUserStrategy.Activate(username, activationCode);
        }

        public void ResendActivationCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _resendActivationCodeStrategy.ResendActivationCode(username);
        }

        public void SendPasswordResetCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _sendPasswordCodeStrategy.SendPasswordResetCode(username);
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(passwordCode).IsNotNullOrEmpty();
            Condition.Requires(newPassword).IsNotNullOrEmpty();

            _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
        }

        public void ResendAccountUnlockCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _resendAccountUnlockCodeStrategy.ResendAccountUnlockCode(username);
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(accountUnlockCode).IsNotNullOrEmpty();

            _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
        }

        public UserStatuses GetUserStatus(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            var user = _userReadRepository.Get(username);

            return user.Status;
        }
    }
}
