namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Repository
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Entities;

    public interface ITraineeshipApplicationDiagnosticsRepository
    {
        IEnumerable<TraineeshipApplicationDetail> GetApplicationsForValidCandidatesWithUnsetLegacyId();

        IEnumerable<CandidateTraineeshipApplicationDetail> GetSubmittedApplicationsWithUnsetLegacyId();
    }
}