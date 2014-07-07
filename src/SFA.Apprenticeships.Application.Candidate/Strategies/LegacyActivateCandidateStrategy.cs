namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public class LegacyActivateCandidateStrategy : IActivateCandidateStrategy
    {
        public void ActivateCandidate(string username, string activationCode)
        {
            //todo: validate activation code
            //todo: update user status to "activated"
            //todo: create candidate in legacy
            //todo: update candidate with legacy candidate id

            throw new NotImplementedException();
        }
    }
}
