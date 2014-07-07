namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateCandidate
{
    using System;
    using Application.Candidate.Strategies;
    using Domain.Entities.Candidates;

    public class LegacyCandidateProvider : ILegacyCandidateProvider
    {
        public int CreateCandidate(Candidate candidate)
        {
            //todo: LegacyCandidateProvider w/s integration
            throw new NotImplementedException();
        }
    }
}
