namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using Domain.Entities.Applications;
    using ActionResults;
    using Attributes;
    using Constants;
    using Constants.Pages;
    using Providers;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using Common.Attributes;
    using Common.Constants;
    using Common.Models.Application;

    [TraineeshipsToggle]
    public class TraineeshipApplicationController : CandidateControllerBase
    {
        private readonly ITraineeshipApplicationProvider _traineeshipApplicationProvider;

        public TraineeshipApplicationController(ITraineeshipApplicationProvider traineeshipApplicationProvider)
        {
            _traineeshipApplicationProvider = traineeshipApplicationProvider;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Apply(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _traineeshipApplicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                //if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                //{
                //    return new VacancyNotFoundResult();
                //}

                //if (model.HasError())
                //{
                //    return RedirectToRoute(CandidateRouteNames.MyApplications);
                //}

                model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

                return View(model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Submit")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> Apply(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                model = StripApplicationViewModelBeforeValidation(model);

                var savedModel = _traineeshipApplicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                model = _traineeshipApplicationProvider.PatchApplicationViewModel(UserContext.CandidateId, savedModel,
                    model);

                var submittedApplicationModel =
                    _traineeshipApplicationProvider.SubmitApplication(UserContext.CandidateId, id, model);

                if (submittedApplicationModel.Status == ApplicationStatuses.ExpiredOrWithdrawn)
                {
                    return new VacancyNotFoundResult();
                }

                if (submittedApplicationModel.ViewModelStatus == ApplicationViewModelStatus.ApplicationInIncorrectState)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }
                if (submittedApplicationModel.ViewModelStatus == ApplicationViewModelStatus.Error)
                {
                    // TODO: change this to something specific to traineeships?
                    SetUserMessage(ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning);
                    return RedirectToAction("Preview", new {id});
                }

                return RedirectToAction("WhatHappensNext",
                    new
                    {
                        id,
                        vacancyReference = submittedApplicationModel.VacancyDetail.VacancyReference,
                        vacancyTitle = submittedApplicationModel.VacancyDetail.Title
                    });
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyQualificationRows")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> AddEmptyQualificationRows(int id, TraineeshipApplicationViewModel model)
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
        public async Task<ActionResult> AddEmptyWorkExperienceRows(int id, TraineeshipApplicationViewModel model)
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

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> WhatHappensNext(int id, string vacancyReference, string vacancyTitle)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _traineeshipApplicationProvider.GetWhatHappensNextViewModel(UserContext.CandidateId, id);

                // TODO: change to something specific to traineeships?
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

        private static TraineeshipApplicationViewModel StripApplicationViewModelBeforeValidation(
            TraineeshipApplicationViewModel model)
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
    }
}