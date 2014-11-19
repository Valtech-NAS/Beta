namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication
{
    using Domain.Interfaces.Repositories;
    using Entities;

    public interface IAuthenticationRepository : IReadRepository<UserCredentials>, IWriteRepository<UserCredentials>
    {
    }
}
