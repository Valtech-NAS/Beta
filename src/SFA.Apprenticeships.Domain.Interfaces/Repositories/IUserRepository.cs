namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IUserReadRepository : IReadRepository<User> {
        User Get(string username);
    }

    public interface IUserWriteRepository : IWriteRepository<User> { }
}
