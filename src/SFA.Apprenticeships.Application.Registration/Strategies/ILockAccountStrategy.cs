namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Domain.Entities.Users;

    public interface ILockAccountStrategy
    {
        void LockAccount(User user);
    }
}
