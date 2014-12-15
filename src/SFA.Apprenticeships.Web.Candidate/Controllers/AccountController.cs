namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Account;

    public class AccountController : CandidateControllerBase
    {
        private readonly IApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator;

        public AccountController(
            IAccountProvider accountProvider,
            SettingsViewModelServerValidator settingsViewModelServerValidator, IApprenticeshipApplicationProvider apprenticeshipApplicationProvider)
        {
            _accountProvider = accountProvider;
            _settingsViewModelServerValidator = settingsViewModelServerValidator;
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Settings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = _accountProvider.GetSettingsViewModel(UserContext.CandidateId);

                return View(model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Settings(SettingsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var validationResult = _settingsViewModelServerValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    ModelState.Clear();
                    validationResult.AddToModelState(ModelState, string.Empty);

                    return View(model);
                }

                var saved = _accountProvider.SaveSettings(UserContext.CandidateId, model);

                if (!saved)
                {
                    ModelState.Clear();
                    SetUserMessage(AccountPageMessages.SettingsUpdateFailed, UserMessageLevel.Warning);

                    return View(model);
                }

                UserData.SetUserContext(UserContext.UserName, model.Firstname + " " + model.Lastname);
                SetUserMessage(AccountPageMessages.SettingsUpdated);

                return RedirectToRoute(CandidateRouteNames.Settings);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var deletedVacancyId = UserData.Pop(UserDataItemNames.DeletedVacancyId);

                if (!string.IsNullOrEmpty(deletedVacancyId))
                {
                    ViewBag.VacancyId = deletedVacancyId;
                }

                var deletedVacancyTitle = UserData.Pop(UserDataItemNames.DeletedVacancyTitle);

                if (!string.IsNullOrEmpty(deletedVacancyTitle))
                {
                    ViewBag.VacancyTitle = deletedVacancyTitle;
                }

                var model = _apprenticeshipApplicationProvider.GetMyApplications(UserContext.CandidateId);

                if (model.ShouldShowTraineeshipsPrompt)
                {
                    ViewBag.RenderTraineeshipsPrompt = true;
                }

                return View(model);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Archive(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var applicationViewModel = _apprenticeshipApplicationProvider.ArchiveApplication(UserContext.CandidateId, id);

                if (applicationViewModel.HasError())
                {
                    SetUserMessage(applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);

                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Delete(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var viewModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

                var applicationViewModel = _apprenticeshipApplicationProvider.DeleteApplication(UserContext.CandidateId, id);

                if (applicationViewModel.HasError())
                {
                    SetUserMessage(applicationViewModel.ViewModelMessage, UserMessageLevel.Warning);

                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                if (viewModel.HasError())
                {
                    SetUserMessage(MyApplicationsPageMessages.ApplicationDeleted);
                }
                else
                {
                    UserData.Push(UserDataItemNames.DeletedVacancyId,
                        viewModel.VacancyDetail.Id.ToString(CultureInfo.InvariantCulture));
                    UserData.Push(UserDataItemNames.DeletedVacancyTitle, viewModel.VacancyDetail.Title);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }
    }
}
