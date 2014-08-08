namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Providers;

    public class MyApplicationsController : SfaControllerBase
    {
        private readonly IApplicationProvider _applicationProvider;

        public MyApplicationsController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider,
            IApplicationProvider applicationProvider
            )
            : base(session, userServiceProvider)
        {
            _applicationProvider = applicationProvider;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            var candidateId = new Guid(User.Identity.Name); // TODO: REFACTOR: move to UserContext?
            var model = _applicationProvider.GetMyApplications(candidateId);

            return View(model);
        }
    }
}
