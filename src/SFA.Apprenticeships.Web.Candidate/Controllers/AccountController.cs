namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Providers;

    public class AccountController : CandidateControllerBase
    {
        public AccountController(ISessionStateProvider session, IUserServiceProvider userServiceProvider)
            : base(session, userServiceProvider) {}

        public ActionResult Index()
        {
            return View();
        }
    }
}
