namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Providers;
    using ViewModels.MyApplications;

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
            return View(GetMyApplicationsViewModel());
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Resume(int vacancyId)
        {
            // TODO: US154: AG: resume draft application.
            throw new NotImplementedException();
        }

        #region Helpers

        private MyApplicationsViewModel GetMyApplicationsViewModel()
        {
            var candidateId = new Guid(User.Identity.Name);
            var model = _applicationProvider.GetMyApplications(candidateId);

            return model;
        }

        #endregion
    }
}
