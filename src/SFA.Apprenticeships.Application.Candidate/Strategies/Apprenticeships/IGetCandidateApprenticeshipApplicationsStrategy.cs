namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public interface IGetCandidateApprenticeshipApplicationsStrategy
    {
        IList<ApprenticeshipApplicationSummary> GetApplications(Guid candidateId);
    }
}
