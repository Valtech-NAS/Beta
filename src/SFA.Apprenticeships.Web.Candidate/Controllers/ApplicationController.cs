namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Applications;

    public class ApplicationController : SfaControllerBase
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ApplicationViewModelServerValidator _validator;

        public ApplicationController(ISessionStateProvider sessionState,
            IUserServiceProvider userServiceProvider,
            IApplicationProvider applicationProvider,
            ApplicationViewModelServerValidator validator)
            : base(sessionState, userServiceProvider)
        {
            _applicationProvider = applicationProvider;
            _validator = validator;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Apply(int id)
        {
            var candidateId = new Guid(User.Identity.Name); // TODO: REFACTOR: move to UserContext?
            var model = _applicationProvider.GetApplicationViewModel(id, candidateId);

            if (model == null)
            {
                Response.StatusCode = 404;
                return View("VacancyNotFound");
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Apply(int id, ApplicationViewModel applicationViewModel)
        {
            var result = _validator.Validate(applicationViewModel);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState, string.Empty);

                return View(applicationViewModel);
            }

            _applicationProvider.SaveApplication(applicationViewModel);

            return RedirectToAction("Preview", new { applicationId = applicationViewModel.ApplicationViewId });
        }

        public ActionResult Preview(string applicationId)
        {
            try
            {
                var applicationViewId = Guid.Parse(applicationId);
                var model = _applicationProvider.GetApplication(applicationViewId);

                // ViewBag.ApplicationViewId is used to provide 'Amend Details' backlinks to the Apply view.
                ViewBag.ApplicationViewId = applicationId;

                return View(model);
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                return View("VacancyNotFound");
            }
        }

        public ActionResult SubmitApplication(string applicationId)
        {
            try
            {
                var applicationViewId = Guid.Parse(applicationId);
                _applicationProvider.SubmitApplication(applicationViewId);

                return RedirectToAction("WhatHappensNext", new { applicationId });
            }
            catch (Exception)
            {
                return RedirectToAction("Preview", new { applicationId });
            }
        }

        public ActionResult WhatHappensNext(string applicationId)
        {
            var applicationViewId = Guid.Parse(applicationId);
            var model = _applicationProvider.GetSubmittedApplicationVacancySummary(applicationViewId);
            return View(model);
        }
    }
}
