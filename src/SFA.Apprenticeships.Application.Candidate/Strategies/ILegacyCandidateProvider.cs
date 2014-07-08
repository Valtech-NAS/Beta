namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface ILegacyCandidateProvider
    {
        int CreateCandidate(Candidate candidate);
    }
}
