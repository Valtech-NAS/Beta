namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Applications;

    public class ApplicationController : CandidateControllerBase
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ApplicationViewModelServerValidator _applicationViewModelFullValidator;
        private readonly ApplicationViewModelSaveValidator _applicationViewModelSaveValidator;

        public ApplicationController(
            IApplicationProvider applicationProvider,
            ApplicationViewModelServerValidator applicationViewModelFullValidator,
            ApplicationViewModelSaveValidator applicationViewModelSaveValidator)
        {
            _applicationProvider = applicationProvider;
            _applicationViewModelFullValidator = applicationViewModelFullValidator;
            _applicationViewModelSaveValidator = applicationViewModelSaveValidator;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            var model = _applicationProvider.GetMyApplications(UserContext.CandidateId);

            return View(model);
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Resume(int id)
        {
            var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (model == null || model.HasError())
            {
                SetUserMessage(model.ViewModelMessage, UserMessageLevel.Warning);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Apply", new { id });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Archive(int id)
        {
            _applicationProvider.ArchiveApplication(UserContext.CandidateId, id);

            SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);

            return RedirectToAction("Index");
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Apply(int id)
        {
            var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (model == null)
            {
                return new VacancyNotFoundResult();
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ValidateInput(false)]
        public ActionResult Apply(int id, ApplicationViewModel applicationViewModel)
        {
            var savedApplicationViewModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (savedApplicationViewModel == null)
            {
                return new VacancyNotFoundResult();
            }

            ModelState.Clear();

            if (savedApplicationViewModel.HasError())
            {
                ShowErrorMessageToUser(applicationViewModel);

                return View("Apply", applicationViewModel);
            }

            var result = applicationViewModel.ApplicationAction == ApplicationAction.Preview
                ? _applicationViewModelFullValidator.Validate(applicationViewModel)
                : _applicationViewModelSaveValidator.Validate(applicationViewModel);

            applicationViewModel = _applicationProvider.PatchApplicationViewModel(
                UserContext.CandidateId, savedApplicationViewModel, applicationViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, string.Empty);

                return View("Apply", applicationViewModel);
            }

            _applicationProvider.SaveApplication(UserContext.CandidateId, id, applicationViewModel);

            if (applicationViewModel.ApplicationAction == ApplicationAction.Preview)
            {
                return RedirectToAction("Preview", new {id});
            }

            applicationViewModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            return View("Apply", applicationViewModel);
        }

        private void ShowErrorMessageToUser(ApplicationViewModel applicationViewModel)
        {
            if (applicationViewModel.ApplicationAction == ApplicationAction.Preview)
            {
                SetUserMessage(ApplicationPageMessages.PreviewFailed, UserMessageLevel.Warning);
            }
            else
            {
                SetUserMessage(ApplicationPageMessages.SaveFailed, UserMessageLevel.Warning);
            }
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Preview(int id)
        {
            try
            {
                var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                ViewBag.VacancyId = id; // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.

                return View(model);
            }
            catch (Exception)
            {
                return new VacancyNotFoundResult();
            }
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult SubmitApplication(int id)
        {
            try
            {
                _applicationProvider.SubmitApplication(UserContext.CandidateId, id);

                return RedirectToAction("WhatHappensNext", new { id });
            }
            catch (Exception)
            {
                SetUserMessage(PreviewPageMessages.SubmissionFailed, UserMessageLevel.Error);

                return RedirectToAction("Preview", new { id });
            }
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult WhatHappensNext(int id)
        {
            var model = _applicationProvider.GetSubmittedApplicationVacancySummary(UserContext.CandidateId, id);

            return View(model);
        }
    }
}
