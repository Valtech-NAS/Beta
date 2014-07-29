namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class UnlockAccountStrategy : IUnlockAccountStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private ISendAccountUnlockCodeStrategy _resendAccountUnlockCodeStrategy;

        public UnlockAccountStrategy(
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ISendAccountUnlockCodeStrategy resendAccountUnlockCodeStrategy)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _resendAccountUnlockCodeStrategy = resendAccountUnlockCodeStrategy;
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Cannot unlock an account that is not locked.", UserStatuses.Locked);

            if (!user.AccountUnlockCode.Equals(accountUnlockCode, StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: AG: US444: EXCEPTION: should use an application exception type
                throw new Exception("Invalid account unlock code.");
            }

            if (user.ActivateCodeExpiry < DateTime.Now)
            {
                // TODO: AG: US444: EXCEPTION: should use an application exception type
                _resendAccountUnlockCodeStrategy.SendAccountUnlockCode(username);

                throw new Exception("Account unlock code has expired, new account unlock code has been sent.");
            }

            user.SetStateActive();

            _userWriteRepository.Save(user);
        }
    }
}
