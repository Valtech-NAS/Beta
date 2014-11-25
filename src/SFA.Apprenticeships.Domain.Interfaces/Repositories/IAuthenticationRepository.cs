namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IAuthenticationRepository : IReadRepository<UserCredentials>, IWriteRepository<UserCredentials>
    {
        UserCredentials Get(Guid id, bool errorIfNotFound);
    }
}
