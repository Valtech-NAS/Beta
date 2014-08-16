namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Providers;

    public class AccountController : CandidateControllerBase
    {
        public AccountController(ISessionStateProvider session) : base(session) {}

        public ActionResult Index()
        {
            return View();
        }
    }
}
