namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;

    public interface ISaveCandidateStrategy
    {
        Candidate SaveCandidate(Candidate candidate);
    }
}