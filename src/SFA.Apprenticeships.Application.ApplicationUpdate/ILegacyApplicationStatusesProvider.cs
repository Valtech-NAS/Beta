namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Entities;

    public interface ILegacyApplicationStatusesProvider
    {
        IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate);

        int GetApplicationStatusesPageCount();

        IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int page);
    }
}
