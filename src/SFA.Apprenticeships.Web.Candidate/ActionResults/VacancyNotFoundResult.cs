namespace SFA.Apprenticeships.Web.Candidate.ActionResults
{
    using System;
    using Common.ActionResults;

    public class VacancyNotFoundResult : HttpCustomStatusCodeResult
    {
        public VacancyNotFoundResult() : base(410) {}
    }
}
