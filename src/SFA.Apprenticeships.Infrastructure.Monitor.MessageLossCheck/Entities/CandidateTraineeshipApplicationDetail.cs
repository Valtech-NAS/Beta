namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Entities
{
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public class CandidateTraineeshipApplicationDetail
    {
        public Candidate Candidate { get; set; }

        public TraineeshipApplicationDetail TraineeshipApplicationDetail { get; set; }
    }
}