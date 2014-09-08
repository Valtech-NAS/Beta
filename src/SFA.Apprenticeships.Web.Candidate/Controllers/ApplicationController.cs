namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using ActionResults;
    using Attributes;
    using Common.Constants;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
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

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            var model = _applicationProvider.GetMyApplications(UserContext.CandidateId);

            return View(model);
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Resume(int id)
        {
            var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (model.HasError())
            {
                SetUserMessage(model.ViewModelMessage, UserMessageLevel.Warning);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Apply", new { id });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Archive(int id)
        {
            _applicationProvider.ArchiveApplication(UserContext.CandidateId, id);

            SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);

            return RedirectToAction("Index");
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Apply(int id)
        {
            var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            // TODO: VGA: US333: must talk with Scott about what to do in this case. We do not have a fully hydrated view model here.
            //if (model.HasError())
            //{
            //    ShowErrorMessageToUser(model);

            //    return View("Apply", model);
            //}

            // TODO: VGA: Consider add this to ViewModel.
            ViewBag.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;
            ViewBag.ConfirmationMessage = ApplicationPageMessages.LeavingPageMessage;

            return View(model);
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ValidateInput(false)]
        public ActionResult Apply(int id, ApplicationViewModel model)
        {
            var savedModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            ModelState.Clear();

            if (savedModel.HasError())
            {
                SetApplicationViewModelUserMessage(model);

                return View("Apply", model);
            }

            var result = model.ApplicationAction == ApplicationAction.Preview
                ? _applicationViewModelFullValidator.Validate(model)
                : _applicationViewModelSaveValidator.Validate(model);

            model = _applicationProvider.PatchApplicationViewModel(
                UserContext.CandidateId, savedModel, model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, string.Empty);

                return View("Apply", model);
            }

            _applicationProvider.SaveApplication(UserContext.CandidateId, id, model);

            if (model.ApplicationAction == ApplicationAction.Preview)
            {
                return RedirectToAction("Preview", new { id });
            }

            // NOTE: we do not check again for an expired or withdrawn vacancy here.
            model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            return View("Apply", model);
        }

        private void SetApplicationViewModelUserMessage(ApplicationViewModel model)
        {
            var message = model.ApplicationAction == ApplicationAction.Preview
                ? ApplicationPageMessages.PreviewFailed
                : ApplicationPageMessages.SaveFailed;

            SetUserMessage(message, UserMessageLevel.Warning);
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public ActionResult Preview(int id)
        {
            var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.
            ViewBag.VacancyId = id;

            return View(model);
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult SubmitApplication(int id)
        {
            try
            {
                var applicationViewModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);
                if (applicationViewModel.Status == ApplicationStatuses.Draft)
                {
                    _applicationProvider.SubmitApplication(UserContext.CandidateId, id);
                }

                return RedirectToAction("WhatHappensNext", new { id });
            }
            catch (Exception)
            {
                SetUserMessage(PreviewPageMessages.SubmissionFailed, UserMessageLevel.Error);

                return RedirectToAction("Preview", new { id });
            }
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult WhatHappensNext(int id)
        {
            var model = _applicationProvider.GetSubmittedApplicationVacancySummary(UserContext.CandidateId, id);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            return View(model);
        }
    }
}
