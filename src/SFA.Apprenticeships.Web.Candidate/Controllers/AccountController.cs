namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;

    public class AccountController : CandidateControllerBase
    {
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
