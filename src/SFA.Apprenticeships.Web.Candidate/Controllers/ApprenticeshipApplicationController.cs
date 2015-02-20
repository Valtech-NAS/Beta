namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Application;
    using ViewModels.Applications;

    [UserJourneyContext(UserJourney.Apprenticeship, Order = 2)]
    public class ApprenticeshipApplicationController : CandidateControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;

        public ApprenticeshipApplicationController(IApprenticeshipApplicationMediator apprenticeshipApplicationMediator)
        {
            _apprenticeshipApplicationMediator = apprenticeshipApplicationMediator;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Resume(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.Resume(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Resume.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Resume.Ok:
                        return RedirectToAction("Apply", response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Apply(string id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.Apply(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Apply.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new {id});
                    case ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Apply.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Apply.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Preview")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Apply(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.PreviewAndSubmit(UserContext.CandidateId, id, model);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.PreviewAndSubmit.Ok:
                        ModelState.Clear();
                        return RedirectToAction("Preview", response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Save")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Save(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.Save(UserContext.CandidateId, id, model);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Save.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.Save.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Save.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View("Apply", response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Save.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Apply", response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Save.Ok:
                        ModelState.Clear();
                        return View("Apply", response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<JsonResult> AutoSave(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run(() =>
            {
                var response = _apprenticeshipApplicationMediator.AutoSave(UserContext.CandidateId, id, model);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.VacancyNotFound:
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.HasError:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.ValidationError:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case ApprenticeshipApplicationMediatorCodes.AutoSave.Ok:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyQualificationRows")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> AddEmptyQualificationRows(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.AddEmptyQualificationRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "AddEmptyWorkExperienceRows")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> AddEmptyWorkExperienceRows(int id, ApprenticeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.AddEmptyWorkExperienceRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Preview(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.Preview(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Preview.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Preview.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Preview.Ok:
                        // ViewBag.VacancyId is used to provide 'Amend Details' backlinks to the Apply view.
                        ViewBag.VacancyId = id;
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> SubmitApplication(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.Submit(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.Submit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Apply", response.ViewModel);
                    case ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.Submit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case ApprenticeshipApplicationMediatorCodes.Submit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToAction("Preview", response.Parameters);
                    case ApprenticeshipApplicationMediatorCodes.Submit.Ok:
                        return RedirectToAction("WhatHappensNext", response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> WhatHappensNext(string id, string vacancyReference, string vacancyTitle)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.WhatHappensNext(UserContext.CandidateId, id, vacancyReference, vacancyTitle);

                switch (response.Code)
                {
                    case ApprenticeshipApplicationMediatorCodes.WhatHappensNext.OfflineVacancy:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipApplicationMediatorCodes.WhatHappensNext.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}