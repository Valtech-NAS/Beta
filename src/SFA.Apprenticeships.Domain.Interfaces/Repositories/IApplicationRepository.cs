namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities.Applications;

    public interface IApplicationReadRepository : IReadRepository<ApplicationDetail>
    {
        ApplicationDetail Get(Guid id, bool errerrorIfNotFound);
        ApplicationDetail Get(Expression<Func<ApplicationDetail, bool>> filter);

        IList<ApplicationSummary> GetForCandidate(Guid candidateId);

        ApplicationDetail GetForCandidate(Guid candidateId, Func<ApplicationDetail, bool> filter);
    }

    public interface IApplicationWriteRepository : IWriteRepository<ApplicationDetail> {
        void ExpireOrWithdrawForCandidate(Guid value, int vacancyId);
    }
}
