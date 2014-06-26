namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities;

    public interface IWriteRepository<in T> where T : BaseEntity
    {
        void Insert(T entity);
        void Delete(T entity);
        void DeleteById(int id);
        void Update(T entity);
    }
}
