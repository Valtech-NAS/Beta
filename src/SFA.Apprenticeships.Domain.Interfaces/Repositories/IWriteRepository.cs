namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities;

    public interface IWriteRepository<T> where T : BaseEntity
    {
        void Delete(int id);
        T Save(T entity);
    }
}
