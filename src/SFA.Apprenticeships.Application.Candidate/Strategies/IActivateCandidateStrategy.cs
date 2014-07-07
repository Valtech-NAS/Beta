namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IActivateCandidateStrategy
    {
        void ActivateCandidate(string username, string activationCode);
    }
}
