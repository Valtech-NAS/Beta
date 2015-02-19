namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Entities;

    public interface ILegacyApplicationStatusesProvider
    {
        IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate);

        int GetApplicationStatusesPageCount(int applicationStatusExtractWindow);

        IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int pageNumber, int applicationStatusExtractWindow);
    }
}
