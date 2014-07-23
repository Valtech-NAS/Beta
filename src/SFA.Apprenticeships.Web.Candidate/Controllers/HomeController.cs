namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using Common.Controllers;
    using Common.Providers;
    using System.Web.Mvc;

    public class HomeController : SfaControllerBase
    {
        public HomeController(ISessionStateProvider session, IUserServiceProvider userServiceProvider)
            : base(session, userServiceProvider) {}

        public ActionResult Index()
        {
            return View();
        }
    }
}
