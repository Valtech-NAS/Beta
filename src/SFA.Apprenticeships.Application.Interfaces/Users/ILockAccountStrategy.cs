namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;
    using Domain.Entities.Users;

    public interface ILockAccountStrategy
    {
        void LockAccount(User user);
    }
}
