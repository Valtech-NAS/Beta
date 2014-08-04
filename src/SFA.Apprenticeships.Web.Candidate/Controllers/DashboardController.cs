namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Providers;

    public class DashboardController : SfaControllerBase
    {
        public DashboardController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider)
            : base(session, userServiceProvider)
        {

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
