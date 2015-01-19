namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Applications;

    public interface ITraineeshipApplicationReadRepository : IReadRepository<TraineeshipApplicationDetail>
    {
        TraineeshipApplicationDetail Get(Guid id, bool errorIfNotFound);

        TraineeshipApplicationDetail Get(int legacyApplicationId);

        IList<TraineeshipApplicationSummary> GetForCandidate(Guid candidateId);

        TraineeshipApplicationDetail GetForCandidate(Guid candidateId, int vacancyId, bool errorIfNotFound = false);

        IEnumerable<TraineeshipApplicationSummary> GetApplicationSummaries(int vacancyId);
    }

    public interface ITraineeshipApplicationWriteRepository : IWriteRepository<TraineeshipApplicationDetail>
    {
    }
}
