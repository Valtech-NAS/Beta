namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;

    public interface IRegisterUserStrategy
    {
        void Register(string username, Guid userId, string activationCode, UserRoles roles);
    }
}
