namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System;
    using Domain.Entities.Candidates;

    public class CandidateBuilder
    {
        private readonly Guid _candidateId;

        public CandidateBuilder(Guid candidateId)
        {
            _candidateId = candidateId;
        }

        public Candidate Build()
        {
            var candidate = new Candidate
            {
                EntityId = _candidateId
            };

            return candidate;
        }
    }
}