namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Domain.Entities.Users;

    public interface IRegisterUserStrategy
    {
        void Register(string username, Guid userId, string activationCode, UserRoles roles);
    }
}
