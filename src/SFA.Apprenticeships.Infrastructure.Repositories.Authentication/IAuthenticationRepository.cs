namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication
{
    using System;
    using Domain.Interfaces.Repositories;
    using Entities;

    public interface IAuthenticationRepository : IReadRepository<UserCredentials>, IWriteRepository<UserCredentials>
    {
        UserCredentials Get(Guid id, bool errorIfNotFound);
    }
}
