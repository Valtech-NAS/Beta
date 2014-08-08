namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;

    public class MyApplicationsController : SfaControllerBase
    {
        public MyApplicationsController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider)
            : base(session, userServiceProvider)
        {
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
