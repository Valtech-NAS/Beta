namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using System;
    using Domain.Entities.Applications;

    public class ApprenticeshipApplicationDetailBuilder
    {
        private readonly Guid _candidateId;
        private readonly int _vacancyId;

        public ApprenticeshipApplicationDetailBuilder(Guid candidateId, int vacancyId)
        {
            _candidateId = candidateId;
            _vacancyId = vacancyId;
        }

        public ApprenticeshipApplicationDetail Build()
        {
            var detail = new ApprenticeshipApplicationDetail
            {
                CandidateId = _candidateId,
                Vacancy = new ApprenticeshipSummaryBuilder(_vacancyId).Build()
            };

            return detail;
        }
    }
}