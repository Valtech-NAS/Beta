namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public interface IApplicationStatusUpdater
    {
        void Update(Candidate candidate, IEnumerable<ApplicationStatusSummary> applicationStatuses);
    }
}
