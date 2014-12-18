namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class UnlockAccountStrategy : IUnlockAccountStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;

        public UnlockAccountStrategy(
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Unlock user account", UserStatuses.Locked);

            if (user.AccountUnlockCodeExpiry < DateTime.Now)
            {
                // NOTE: account unlock code has expired, send a new one.
                _sendAccountUnlockCodeStrategy.SendAccountUnlockCode(username);
                throw new CustomException("Account unlock code has expired, new account unlock code has been sent.",
                    ErrorCodes.AccountUnlockCodeExpired);
            }

            if (!user.AccountUnlockCode.Equals(accountUnlockCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new CustomException("Invalid account unlock code.", ErrorCodes.AccountUnlockCodeInvalid);
            }

            user.SetStateActive();
            _userWriteRepository.Save(user);
        }
    }
}
