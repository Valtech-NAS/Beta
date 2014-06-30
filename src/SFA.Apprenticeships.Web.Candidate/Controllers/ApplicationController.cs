namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Infrastructure.Azure.Session;
    using Providers;

    public class ApplicationController : SfaControllerBase
    {
        private readonly IApplicationProvider _applicationProvider;

        public ApplicationController(ISessionState sessionState, IApplicationProvider applicationProvider) : base(sessionState)
        {
            _applicationProvider = applicationProvider;
        }

        public ActionResult Index(int id)
        {
            return View(id);
        }

        public ActionResult Apply(int id, int profileId)
        {
            var applicationViewModel = _applicationProvider.GetApplicationViewModel(id, profileId);

            if (applicationViewModel == null)
            {
                Response.StatusCode = 404;
                return View("vacancynotfound");
            }

            return View(applicationViewModel);
        }
    }
}