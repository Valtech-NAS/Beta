namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public interface ILegacyCandidateApplicationStatusesProvider
    {
        IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate);

        IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses();
    }
}
