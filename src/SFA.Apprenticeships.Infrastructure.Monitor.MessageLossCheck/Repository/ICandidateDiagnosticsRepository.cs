namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Repository
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    public interface ICandidateDiagnosticsRepository
    {
        IEnumerable<Candidate> GetActivatedCandidatesWithUnsetLegacyId();
    }
}