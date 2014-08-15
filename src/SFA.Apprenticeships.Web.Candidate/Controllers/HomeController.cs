namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using Common.Providers;
    using System.Web.Mvc;

    public class HomeController : CandidateControllerBase
    {
        public HomeController(ISessionStateProvider session, IUserServiceProvider userServiceProvider)
            : base(session, userServiceProvider) {}

        public ActionResult Index()
        {
            return View();
        }
    }
}
