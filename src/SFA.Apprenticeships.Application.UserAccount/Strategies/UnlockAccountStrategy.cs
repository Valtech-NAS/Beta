namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

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

            user.AssertState("Cannot unlock an account that is not locked.", UserStatuses.Locked);

            if (user.AccountUnlockCodeExpiry < DateTime.Now)
            {
                // NOTE: account unlock code has expired, send a new one.
                _sendAccountUnlockCodeStrategy.SendAccountUnlockCode(username);
                throw new Exception("Account unlock code has expired, new account unlock code has been sent.");
            }

            if (!user.AccountUnlockCode.Equals(accountUnlockCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Invalid account unlock code.");
            }

            // 
            user.SetStateActive();
            _userWriteRepository.Save(user);
        }
    }
}
