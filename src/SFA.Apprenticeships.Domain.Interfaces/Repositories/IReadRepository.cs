namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities;

    public interface IReadRepository<T> where T : BaseEntity
    {
        T Get(Guid id);
    }
}
