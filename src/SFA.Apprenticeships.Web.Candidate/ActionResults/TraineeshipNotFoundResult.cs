namespace SFA.Apprenticeships.Web.Candidate.ActionResults
{
    using Common.ActionResults;

    public class TraineeshipNotFoundResult : HttpCustomStatusCodeResult
    {
        public TraineeshipNotFoundResult() : base(410, 2)
        {
        }
    }
}