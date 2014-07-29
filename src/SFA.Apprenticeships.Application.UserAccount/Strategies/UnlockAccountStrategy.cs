namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class UnlockAccountStrategy : IUnlockAccountStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ILockAccountStrategy _lockAccountStrategy;

        public UnlockAccountStrategy(
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ILockAccountStrategy lockAccountStrategy)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _lockAccountStrategy = lockAccountStrategy;
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Cannot unlock an account that is not locked.", UserStatuses.Locked);

            if (user.AccountUnlockCodeExpiry < DateTime.Now)
            {
                // NOTE: re-locking the account generates a new code with a new expiry date
                // and sends an email.
                _lockAccountStrategy.LockAccount(user);

                // TODO: AG: US444: EXCEPTION: should use an application exception type
                throw new Exception("Account unlock code has expired, new account unlock code has been sent.");
            }

            if (!user.AccountUnlockCode.Equals(accountUnlockCode, StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: AG: US444: EXCEPTION: should use an application exception type
                throw new Exception("Invalid account unlock code.");
            }

            user.SetStateActive();

            _userWriteRepository.Save(user);
        }
    }
}
