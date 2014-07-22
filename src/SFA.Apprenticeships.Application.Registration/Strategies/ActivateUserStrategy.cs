namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class ActivateUserStrategy : IActivateUserStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public ActivateUserStrategy(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public void Activate(string username, string activationCode)
        {
            var user = _userReadRepository.Get(username);

            // TODO: NOTIMPL: check status of user (only allowed if pending activation)

            if (!user.ActivationCode.Equals(activationCode, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Invalid activation code"); // TODO: EXCEPTION: should use an application exception type

            user.Status = UserStatuses.Active;
            user.ActivationCode = string.Empty;
            user.ActivateCodeExpiry = null;
            user.LoginRemainingAttempts = 3; //todo: value from config/setting
            user.PasswordResetCode = string.Empty;
            user.PasswordResetCodeExpiry = null;
            user.PasswordResetRemainingAttempts = 3; //todo: value from config/setting

            _userWriteRepository.Save(user);
        }
    }
}
