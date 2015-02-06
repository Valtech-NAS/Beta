namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;

    public interface IAuthenticateCandidateStrategy
    {
        Candidate AuthenticateCandidate(string username, string password);
    }
}
