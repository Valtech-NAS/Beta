namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Applications;

    public interface IApplicationReadRepository : IReadRepository<ApplicationDetail>
    {
        IList<ApplicationSummary> GetForCandidate(Guid candidateId);
    }

    public interface IApplicationWriteRepository : IWriteRepository<ApplicationDetail> { }
}
