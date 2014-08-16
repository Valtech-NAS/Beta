namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using Common.Providers;
    using System.Web.Mvc;

    public class HomeController : CandidateControllerBase
    {
        public HomeController(ISessionStateProvider session) : base(session) {}

        public ActionResult Index()
        {
            return View();
        }
    }
}
