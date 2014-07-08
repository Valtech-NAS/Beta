namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using FluentValidation.Mvc;
    using Infrastructure.Azure.Session;
    using Providers;
    using Validators;
    using ViewModels.Applications;

    public class ApplicationController : SfaControllerBase
    {
        private const string TempAppFormSessionId = "TempAppForm";

        private readonly IApplicationProvider _applicationProvider;
        private readonly ApplicationViewModelServerValidator _validator;

        public ApplicationController(ISessionState sessionState, 
                                    IApplicationProvider applicationProvider,
                                    ApplicationViewModelServerValidator validator)
            : base(sessionState)
        {
            _applicationProvider = applicationProvider;
            _validator = validator;
        }

        public ActionResult Index(int id)
        {
            return View(id);
        }

        public ActionResult Apply(int id, int mockProfileId)
        {
            var applicationViewModel = _applicationProvider.GetApplicationViewModel(id, mockProfileId);

            if (applicationViewModel == null)
            {
                Response.StatusCode = 404;
                return View("VacancyNotFound");
            }

            return View(applicationViewModel);
        }

        [HttpPost]
        public ActionResult Apply(int id, int mockProfileId, ApplicationViewModel applicationViewModel)
        {
            applicationViewModel = _applicationProvider.MergeApplicationViewModel(id, mockProfileId, applicationViewModel);
            
            var result = _validator.Validate(applicationViewModel);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState, string.Empty);
                return View(applicationViewModel);
            }

            Session.Store(TempAppFormSessionId, applicationViewModel);
            return RedirectToAction("Preview", new { id = applicationViewModel.VacancyDetail.Id });
        }

        public ActionResult Preview(int id)
        {
            var appForm = Session.Get<ApplicationViewModel>(TempAppFormSessionId);
            return View(appForm);
        }
    }
}