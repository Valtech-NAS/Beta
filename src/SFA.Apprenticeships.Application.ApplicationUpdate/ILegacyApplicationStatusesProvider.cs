namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public interface ILegacyApplicationStatusesProvider
    {
        IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate);

        IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses();
    }
}
