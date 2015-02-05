namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System;
    using Domain.Entities.Applications;

    public class ApprenticeshipApplicationDetailBuilder
    {
        private readonly Guid _candidateId;

        public ApprenticeshipApplicationDetailBuilder(Guid candidateId, int vacancyId)
        {
            _candidateId = candidateId;
        }

        public ApprenticeshipApplicationDetail Build()
        {
            var detail = new ApprenticeshipApplicationDetail
            {
                CandidateId = _candidateId
            };

            return detail;
        }
    }
}