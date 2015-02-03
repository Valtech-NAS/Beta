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
    using ViewModels.Applications;

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
                    case Codes.ApprenticeshipApplication.Resume.HasError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case Codes.ApprenticeshipApplication.Resume.Ok:
                        return RedirectToAction("Apply", response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Apply(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.Apply(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case Codes.ApprenticeshipApplication.Apply.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipApplication.Apply.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case Codes.ApprenticeshipApplication.Apply.Ok:
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
                    case Codes.ApprenticeshipApplication.PreviewAndSubmit.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipApplication.PreviewAndSubmit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case Codes.ApprenticeshipApplication.PreviewAndSubmit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case Codes.ApprenticeshipApplication.PreviewAndSubmit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case Codes.ApprenticeshipApplication.PreviewAndSubmit.Ok:
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
                    case Codes.ApprenticeshipApplication.Save.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipApplication.Save.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View("Apply", response.ViewModel);
                    case Codes.ApprenticeshipApplication.Save.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Apply", response.ViewModel);
                    case Codes.ApprenticeshipApplication.Save.Ok:
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
                    case Codes.ApprenticeshipApplication.AutoSave.VacancyNotFound:
                        return new JsonResult { Data = response.ViewModel };
                    case Codes.ApprenticeshipApplication.AutoSave.HasError:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case Codes.ApprenticeshipApplication.AutoSave.ValidationError:
                        ModelState.Clear();
                        return new JsonResult { Data = response.ViewModel };
                    case Codes.ApprenticeshipApplication.AutoSave.Ok:
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
                    case Codes.ApprenticeshipApplication.Preview.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipApplication.Preview.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case Codes.ApprenticeshipApplication.Preview.Ok:
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
                    case Codes.ApprenticeshipApplication.PreviewAndSubmit.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Apply", response.ViewModel);
                    case Codes.ApprenticeshipApplication.Submit.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipApplication.Submit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case Codes.ApprenticeshipApplication.Submit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToAction("Preview", response.Parameters);
                    case Codes.ApprenticeshipApplication.Submit.Ok:
                        return RedirectToAction("WhatHappensNext", response.Parameters);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> WhatHappensNext(int id, string vacancyReference, string vacancyTitle)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipApplicationMediator.WhatHappensNext(UserContext.CandidateId, id, vacancyReference, vacancyTitle);

                switch (response.Code)
                {
                    case Codes.ApprenticeshipApplication.WhatHappensNext.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case Codes.ApprenticeshipApplication.WhatHappensNext.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}