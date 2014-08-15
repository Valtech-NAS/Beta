namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
    using Providers;
    using Validators;
    using ViewModels.Applications;

    public class ApplicationController : SfaControllerBase
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ApplicationViewModelServerValidator _applicationViewModelFullValidator;
        private readonly ApplicationViewModelSaveValidator _applicationViewModelSaveValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;

        public ApplicationController(ISessionStateProvider sessionState,
            IUserServiceProvider userServiceProvider,
            IApplicationProvider applicationProvider,
            ICandidateServiceProvider candidateServiceProvider,
            ApplicationViewModelServerValidator applicationViewModelFullValidator,
            ApplicationViewModelSaveValidator applicationViewModelSaveValidator)
            : base(sessionState, userServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _applicationProvider = applicationProvider;
            _applicationViewModelFullValidator = applicationViewModelFullValidator;
            _applicationViewModelSaveValidator = applicationViewModelSaveValidator;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            // my application / dashboard
            var candidateId = new Guid(User.Identity.Name); // TODO: REFACTOR: move to UserContext?
            var model = _applicationProvider.GetMyApplications(candidateId);

            return View(model);
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Apply(int id)
        {
            var candidateId = new Guid(User.Identity.Name); // TODO: REFACTOR: move to UserContext?
            var model = _applicationProvider.GetApplicationViewModel(id, candidateId);

            if (model == null)
            {
                return new VacancyNotFoundResult();
            }

            _candidateServiceProvider.LastViewedVacancyId = id;

            return View(model);
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Resume(int id)
        {
            var candidateId = new Guid(User.Identity.Name); // TODO: REFACTOR: move to UserContext
            var model = _applicationProvider.GetApplicationViewModel(id, candidateId);

            if (model == null)
            {
                SetUserMessage(MyApplicationsPageMessages.DraftExpired, UserMessageLevel.Warning);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Apply", new { id });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Archive(int id)
        {
            var candidateId = new Guid(User.Identity.Name); // TODO: REFACTOR: move to UserContext?
            _applicationProvider.ArchiveApplication(id, candidateId);

            SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Apply(int id, ApplicationViewModel applicationViewModel)
        {
            ModelState.Clear();
            ValidationResult result = applicationViewModel.ApplicationAction == ApplicationAction.Preview
                ? _applicationViewModelFullValidator.Validate(applicationViewModel)
                : _applicationViewModelSaveValidator.Validate(applicationViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, string.Empty);
                applicationViewModel = _applicationProvider.GetApplicationViewModel(applicationViewModel);
                return View("Apply", applicationViewModel);
            }

            _applicationProvider.SaveApplication(applicationViewModel);

            if (applicationViewModel.ApplicationAction == ApplicationAction.Preview)
            {
                return RedirectToAction("Preview", new {applicationId = applicationViewModel.ApplicationViewId});
            }
            
            applicationViewModel = _applicationProvider.GetApplicationViewModel(applicationViewModel.ApplicationViewId);
            return View("Apply", applicationViewModel);
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Preview(string applicationId)
        {
            try
            {
                var applicationViewId = Guid.Parse(applicationId);
                var model = _applicationProvider.GetApplicationViewModel(applicationViewId);

                // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.
                ViewBag.VacancyId = model.VacancyDetail.Id;

                return View(model);
            }
            catch (Exception)
            {
                return new VacancyNotFoundResult();
            }
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
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
                SetUserMessage(PreviewPageMessages.SubmissionFailed, UserMessageLevel.Error);

                return RedirectToAction("Preview", new { applicationId });
            }
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult WhatHappensNext(string applicationId)
        {
            var applicationViewId = Guid.Parse(applicationId);
            var model = _applicationProvider.GetSubmittedApplicationVacancySummary(applicationViewId);

            return View(model);
        }
    }
}
