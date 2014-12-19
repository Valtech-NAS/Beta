namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface IGetCandidateTraineeshipApplicationsStrategy
    {
        IList<TraineeshipApplicationSummary> GetApplications(Guid candidateId);
    }
}
