namespace SFA.Apprenticeships.Web.Candidate.ActionResults
{
    using SFA.Apprenticeships.Web.Common.ActionResults;

    public class VacancyNotFoundResult : HttpCustomStatusCodeResult
    {
        public VacancyNotFoundResult() : base(410)
        {
        }
    }
}