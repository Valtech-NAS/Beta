namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Entities
{
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public class CandidateApprenticeshipApplicationDetail
    {
        public Candidate Candidate { get; set; }

        public ApprenticeshipApplicationDetail ApprenticeshipApplicationDetail { get; set; }
    }
}