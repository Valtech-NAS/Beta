namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;

    public class LegacyGetCandidateApplicationsStrategy : IGetCandidateApplicationsStrategy
    {
        public IList<ApplicationSummary> GetApplications(Guid candidateId)
        {
            //todo: need to get latest status of apps for the specified candidate from legacy (assuming low latency so can do this synchronously)
            // (1) call ILegacyCandidateApplicationsProvider.GetCandidateApplicationStatuses
            // (2) call ApplicationUpdate process with results from (1)
            // (3) read updated from Apps repo

            throw new NotImplementedException();
        }
    }
}
