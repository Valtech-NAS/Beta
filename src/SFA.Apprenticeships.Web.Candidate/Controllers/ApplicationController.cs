namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using ActionResults;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Applications;
    using ViewModels.Candidate;

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

            return RedirectToAction("Apply", new {id});
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

            if (model.HasError())
            {
                return RedirectToAction("Index");
            }

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return View(model);
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Preview")]
        [ValidateInput(false)]
        public ActionResult Apply(int id, ApplicationViewModel model)
        {
            model = StripApplicationViewModelBeforeValidation(model);

            var savedModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }
            if (savedModel.Status != ApplicationStatuses.Draft)
            {
                return RedirectToAction("Index");
            }

            ModelState.Clear();

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                SetUserMessage(ApplicationPageMessages.PreviewFailed, UserMessageLevel.Warning);

                return View("Apply", model);
            }

            var result = _applicationViewModelFullValidator.Validate(model);

            model = _applicationProvider.PatchApplicationViewModel(
                UserContext.CandidateId, savedModel, model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, string.Empty);

                return View("Apply", model);
            }

            _applicationProvider.SaveApplication(UserContext.CandidateId, id, model);

            return RedirectToAction("Preview", new {id});
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Save")]
        [ValidateInput(false)]
        public ActionResult Save(int id, ApplicationViewModel model)
        {
            model = StripApplicationViewModelBeforeValidation(model);

            var savedModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

            if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            ModelState.Clear();

            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            if (savedModel.HasError())
            {
                SetUserMessage(ApplicationPageMessages.SaveFailed, UserMessageLevel.Warning);

                return View("Apply", model);
            }

            var result = _applicationViewModelSaveValidator.Validate(model);

            model = _applicationProvider.PatchApplicationViewModel(
                UserContext.CandidateId, savedModel, model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, string.Empty);

                return View("Apply", model);
            }

            _applicationProvider.SaveApplication(UserContext.CandidateId, id, model);

            model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);
            model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

            return View("Apply", model);
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyQualificationRows")]
        [ValidateInput(false)]
        public ActionResult AddEmptyQualificationRows(int id, ApplicationViewModel model)
        {
            model.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(model.Candidate.Qualifications);
            model.Candidate.HasQualifications = model.Candidate.Qualifications.Count() != 0;
            model.DefaultQualificationRows = 5;
            model.DefaultWorkExperienceRows = 0;

            ModelState.Clear();

            return View("Apply", model);
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyWorkExperienceRows")]
        [ValidateInput(false)]
        public ActionResult AddEmptyWorkExperienceRows(int id, ApplicationViewModel model)
        {
            model.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(model.Candidate.WorkExperience);
            model.Candidate.HasWorkExperience = model.Candidate.WorkExperience.Count() != 0;

            model.DefaultQualificationRows = 0;
            model.DefaultWorkExperienceRows = 3;

            ModelState.Clear();

            return View("Apply", model);
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

            if (model.HasError())
            {
                return RedirectToAction("Index");
            }

            // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.
            ViewBag.VacancyId = id;

            return View(model);
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult SubmitApplication(int id)
        {
            var model = _applicationProvider.SubmitApplication(UserContext.CandidateId, id);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            if (!model.HasError()) return RedirectToAction("WhatHappensNext", new {id});
            
            SetUserMessage(ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning);
            return RedirectToAction("Preview", new { id });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult WhatHappensNext(int id)
        {
            var model = _applicationProvider.GetWhatHappensNextViewModel(UserContext.CandidateId, id);

            if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {
                return new VacancyNotFoundResult();
            }

            if (model.HasError())
            {
                SetUserMessage(model.ViewModelMessage, UserMessageLevel.Warning);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        #region Helpers

        private static ApplicationViewModel StripApplicationViewModelBeforeValidation(ApplicationViewModel model)
        {
            model.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(model.Candidate.Qualifications);
            model.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(model.Candidate.WorkExperience);

            model.DefaultQualificationRows = 0;
            model.DefaultWorkExperienceRows = 0;

            if (model.IsJavascript)
            {
                return model;
            }

            model.Candidate.HasQualifications = model.Candidate.Qualifications.Count() != 0;
            model.Candidate.HasWorkExperience = model.Candidate.WorkExperience.Count() != 0;

            return model;
        }

        private static IEnumerable<WorkExperienceViewModel> RemoveEmptyRowsFromWorkExperience(
            IEnumerable<WorkExperienceViewModel> workExperience)
        {
            if (workExperience == null)
            {
                return new List<WorkExperienceViewModel>();
            }

            return workExperience.Where(vm =>
                vm.Employer != null && !string.IsNullOrWhiteSpace(vm.Employer.Trim()) ||
                vm.JobTitle != null && !string.IsNullOrWhiteSpace(vm.JobTitle.Trim()) ||
                vm.Description != null && !string.IsNullOrWhiteSpace(vm.Description.Trim())||
                vm.FromYear != null && !string.IsNullOrWhiteSpace(vm.FromYear.Trim())||
                vm.ToYear != null && !string.IsNullOrWhiteSpace(vm.ToYear.Trim())
                ).ToList();
        }

        private static IEnumerable<QualificationsViewModel> RemoveEmptyRowsFromQualifications(
            IEnumerable<QualificationsViewModel> qualifications)
        {
            if (qualifications == null)
            {
                return new List<QualificationsViewModel>();
            }

            return qualifications.Where(vm =>
                vm.Subject != null && !string.IsNullOrWhiteSpace(vm.Subject.Trim()) ||
                vm.QualificationType != null && !string.IsNullOrWhiteSpace(vm.QualificationType.Trim()) ||
                vm.Grade != null && !string.IsNullOrWhiteSpace(vm.Grade.Trim())||
                vm.Year != null && !string.IsNullOrWhiteSpace(vm.Year.Trim())).ToList();
        }

        #endregion
    }
}