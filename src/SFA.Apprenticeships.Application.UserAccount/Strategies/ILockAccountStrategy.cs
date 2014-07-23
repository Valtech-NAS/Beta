namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;

    public interface ILockAccountStrategy
    {
        void LockAccount(User user);
    }
}
