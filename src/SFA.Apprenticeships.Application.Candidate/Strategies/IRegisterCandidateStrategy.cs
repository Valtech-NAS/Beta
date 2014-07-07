namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;

    public interface IRegisterCandidateStrategy
    {
        Candidate RegisterCandidate(Candidate newCandidate, string password);
    }
}
