namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Applications;

    public interface IApprenticeshipApplicationReadRepository : IReadRepository<ApprenticeshipApplicationDetail>
    {
        ApprenticeshipApplicationDetail Get(Guid id, bool errorIfNotFound);

        ApprenticeshipApplicationDetail Get(int legacyApplicationId);

        IList<ApprenticeshipApplicationSummary> GetForCandidate(Guid candidateId);

        ApprenticeshipApplicationDetail GetForCandidate(Guid candidateId, Func<ApprenticeshipApplicationDetail, bool> filter);
    }

    public interface IApprenticeshipApplicationWriteRepository : IWriteRepository<ApprenticeshipApplicationDetail> {
        void ExpireOrWithdrawForCandidate(Guid value, int vacancyId);
    }
}
