namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Domain.Entities.Candidates;

    public interface ILegacyCandidateProvider
    {
        int CreateCandidate(Candidate candidate);
    }
}
