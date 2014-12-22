namespace SFA.Apprenticeships.Web.Candidate.ActionResults
{
    using Common.ActionResults;

    public class VacancyNotFoundResult : HttpCustomStatusCodeResult
    {
        public VacancyNotFoundResult() : base(410)
        {
        }
    }
}