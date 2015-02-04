namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using Domain.Entities.Users;

    public class LockAccountStrategy : ILockAccountStrategy
    {
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;
        private readonly ILockUserStrategy _lockUserStrategy;

        public LockAccountStrategy(ILockUserStrategy lockUserStrategy, ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy)
        {
            _lockUserStrategy = lockUserStrategy;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
        }

        public void LockAccount(User user)
        {
            _lockUserStrategy.LockUser(user);
            _sendAccountUnlockCodeStrategy.SendAccountUnlockCode(user.Username);
        }
    }
}
