namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using FluentValidation.Mvc;
    using Domain.Entities.Applications;
    using ActionResults;
    using Attributes;
    using Constants;
    using Constants.Pages;
    using Helpers;
    using Providers;
    using Validators;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using Common.Attributes;
    using Common.Constants;
    using Common.Models.Application;

    public class ApprenticeshipApplicationController : CandidateControllerBase
    {
        private readonly IApplicationProvider _applicationProvider;
        private readonly ApprenticeshipApplicationViewModelServerValidator _apprenticeshipApplicationViewModelFullValidator;
        private readonly ApprenticeshipApplicationViewModelSaveValidator _apprenticeshipApplicationViewModelSaveValidator;

        public ApprenticeshipApplicationController(
            IApplicationProvider applicationProvider,
            ApprenticeshipApplicationViewModelServerValidator apprenticeshipApplicationViewModelFullValidator,
            ApprenticeshipApplicationViewModelSaveValidator apprenticeshipApplicationViewModelSaveValidator)
        {
            _applicationProvider = applicationProvider;
            _apprenticeshipApplicationViewModelFullValidator = apprenticeshipApplicationViewModelFullValidator;
            _apprenticeshipApplicationViewModelSaveValidator = apprenticeshipApplicationViewModelSaveValidator;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Resume(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                if (model.HasError())
                {
                    SetUserMessage(model.ViewModelMessage, UserMessageLevel.Warning);
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                return RedirectToAction("Apply", new {id});
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Apply(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new VacancyNotFoundResult();
                }

                if (model.HasError())
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

                return View(model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Preview")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> Apply(int id, ApprenticheshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                model = StripApplicationViewModelBeforeValidation(model);

                var savedModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new VacancyNotFoundResult();
                }
                if (savedModel.Status != ApplicationStatuses.Draft)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                ModelState.Clear();

                model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

                if (savedModel.HasError())
                {
                    SetUserMessage(ApplicationPageMessages.PreviewFailed, UserMessageLevel.Warning);

                    return View("Apply", model);
                }

                var result = _apprenticeshipApplicationViewModelFullValidator.Validate(model);

                model = _applicationProvider.PatchApplicationViewModel(
                    UserContext.CandidateId, savedModel, model);

                if (!result.IsValid)
                {
                    result.AddToModelState(ModelState, string.Empty);

                    return View("Apply", model);
                }

                _applicationProvider.SaveApplication(UserContext.CandidateId, id, model);

                return RedirectToAction("Preview", new {id});
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Save")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> Save(int id, ApprenticheshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
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

                var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(model);

                model = _applicationProvider.PatchApplicationViewModel(UserContext.CandidateId, savedModel, model);

                if (!result.IsValid)
                {
                    result.AddToModelState(ModelState, string.Empty);

                    return View("Apply", model);
                }

                _applicationProvider.SaveApplication(UserContext.CandidateId, id, model);

                model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);
                model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

                return View("Apply", model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ValidateInput(false)]
        public async Task<JsonResult> AutoSave(int id, ApprenticheshipApplicationViewModel model)
        {
            return await Task.Run(() =>
            {
                var autoSaveResult = new AutoSaveResultViewModel();

                var savedModel = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                if (savedModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    autoSaveResult.Status = "failed";

                    return new JsonResult {Data = autoSaveResult};
                }

                ModelState.Clear();

                model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

                if (savedModel.HasError())
                {
                    autoSaveResult.Status = "failed";

                    return new JsonResult {Data = autoSaveResult};
                }

                var result = _apprenticeshipApplicationViewModelSaveValidator.Validate(model);

                model = _applicationProvider.PatchApplicationViewModel(
                    UserContext.CandidateId, savedModel, model);

                if (!result.IsValid)
                {
                    autoSaveResult.Status = "failed";

                    return new JsonResult {Data = autoSaveResult};
                }

                _applicationProvider.SaveApplication(UserContext.CandidateId, id, model);

                model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);
                model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

                autoSaveResult.Status = "succeeded";

                if (model.DateUpdated != null)
                {
                    autoSaveResult.DateTimeMessage =
                        AutoSaveDateTimeHelper.GetDisplayDateTime((DateTime) model.DateUpdated);
                }

                return new JsonResult {Data = autoSaveResult};
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyQualificationRows")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> AddEmptyQualificationRows(int id, ApprenticheshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                model.Candidate.Qualifications = RemoveEmptyRowsFromQualifications(model.Candidate.Qualifications);
                model.Candidate.HasQualifications = model.Candidate.Qualifications.Count() != 0;
                model.DefaultQualificationRows = 5;
                model.DefaultWorkExperienceRows = 0;

                ModelState.Clear();

                return View("Apply", model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyWorkExperienceRows")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> AddEmptyWorkExperienceRows(int id, ApprenticheshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                model.Candidate.WorkExperience = RemoveEmptyRowsFromWorkExperience(model.Candidate.WorkExperience);
                model.Candidate.HasWorkExperience = model.Candidate.WorkExperience.Count() != 0;

                model.DefaultQualificationRows = 0;
                model.DefaultWorkExperienceRows = 3;

                ModelState.Clear();

                return View("Apply", model);
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Preview(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new VacancyNotFoundResult();
                }

                if (model.HasError())
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.
                ViewBag.VacancyId = id;

                return View(model);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> SubmitApplication(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _applicationProvider.SubmitApplication(UserContext.CandidateId, id);

                if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new VacancyNotFoundResult();
                }

                if (model.ViewModelStatus == ApplicationViewModelStatus.ApplicationInIncorrectState)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }
                if (model.ViewModelStatus == ApplicationViewModelStatus.Error)
                {
                    SetUserMessage(ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning);
                    return RedirectToAction("Preview", new {id});
                }

                return RedirectToAction("WhatHappensNext",
                    new
                    {
                        id,
                        vacancyReference = model.VacancyDetail.VacancyReference,
                        vacancyTitle = model.VacancyDetail.Title
                    });
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> WhatHappensNext(int id, string vacancyReference, string vacancyTitle)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _applicationProvider.GetWhatHappensNextViewModel(UserContext.CandidateId, id);

                if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new VacancyNotFoundResult();
                }

                if (model.HasError())
                {
                    model.VacancyReference = vacancyReference;
                    model.VacancyTitle = vacancyTitle;
                }

                return View(model);
            });
        }

        #region Helpers

        private static ApprenticheshipApplicationViewModel StripApplicationViewModelBeforeValidation(ApprenticheshipApplicationViewModel model)
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
                vm.Description != null && !string.IsNullOrWhiteSpace(vm.Description.Trim()) ||
                vm.FromYear != null && !string.IsNullOrWhiteSpace(vm.FromYear.Trim()) ||
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
                vm.Grade != null && !string.IsNullOrWhiteSpace(vm.Grade.Trim()) ||
                vm.Year != null && !string.IsNullOrWhiteSpace(vm.Year.Trim())).ToList();
        }

        #endregion
    }
}