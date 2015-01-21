namespace SFA.Apprenticeships.Web.Candidate.ActionResults
{
    using Common.ActionResults;

    public class ApprenticeshipNotFoundResult : HttpCustomStatusCodeResult
    {
        public ApprenticeshipNotFoundResult() : base(410, 1)
        {
        }
    }
}