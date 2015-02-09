namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Constants;
    using Mediators;
    using Mediators.Application;
    using ViewModels.Applications;

    public class TraineeshipApplicationController : CandidateControllerBase
    {
        private readonly ITraineeshipApplicationMediator _traineeshipApplicationMediator;

        public TraineeshipApplicationController(ITraineeshipApplicationMediator traineeshipApplicationMediator)
        {
            _traineeshipApplicationMediator = traineeshipApplicationMediator;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Apply(string id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.Apply(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.Apply.HasError:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case TraineeshipApplicationMediatorCodes.Apply.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [MultipleFormActionsButton(Name = "ApplicationAction", Argument = "Submit")]
        [ApplyWebTrends]
        [ValidateInput(false)]
        [ClearSearchReturnUrl(ClearSearchReturnUrl = false)]
        public async Task<ActionResult> Apply(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.Submit(UserContext.CandidateId, id, model);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.Submit.IncorrectState:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);
                    case TraineeshipApplicationMediatorCodes.Submit.Error:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case TraineeshipApplicationMediatorCodes.Submit.Ok:
                        return RedirectToAction("WhatHappensNext", response.Parameters);
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
        public async Task<ActionResult> AddEmptyQualificationRows(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.AddEmptyQualificationRows(model);

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
        public async Task<ActionResult> AddEmptyWorkExperienceRows(int id, TraineeshipApplicationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.AddEmptyWorkExperienceRows(model);

                ModelState.Clear();

                return View("Apply", response.ViewModel);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> WhatHappensNext(string id, string vacancyReference, string vacancyTitle)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _traineeshipApplicationMediator.WhatHappensNext(UserContext.CandidateId, id, vacancyReference, vacancyTitle);

                switch (response.Code)
                {
                    case TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound:
                        return new TraineeshipNotFoundResult();
                    case TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}