namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Applications;

    public interface IApplicationReadRepository : IReadRepository<ApplicationDetail> { }

    public interface IApplicationWriteRepository : IWriteRepository<ApplicationDetail> { }
}
