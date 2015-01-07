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
    using Mediators;
    using Mediators.Account;
    using ViewModels.Account;

    public class AccountController : CandidateControllerBase
    {
        private readonly IAccountMediator _accountMediator;

        public AccountController(IAccountMediator accountMediator)
        {
            _accountMediator = accountMediator;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Index(UserContext.CandidateId, UserData.Pop(UserDataItemNames.DeletedVacancyId), UserData.Pop(UserDataItemNames.DeletedVacancyTitle));
                return View(response.ViewModel);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Settings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId);
                return View(response.ViewModel);
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
                var response = _accountMediator.Settings(UserContext.CandidateId, model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case Codes.AccountMediator.Settings.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case Codes.AccountMediator.Settings.SaveError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(model);
                    case Codes.AccountMediator.Settings.Success:
                        UserData.SetUserContext(UserContext.UserName, response.ViewModel.Firstname + " " + response.ViewModel.Lastname);
                        SetUserMessage(AccountPageMessages.SettingsUpdated);
                        return RedirectToRoute(CandidateRouteNames.Settings);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Archive(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Archive(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case Codes.AccountMediator.Archive.SuccessfullyArchived:
                        SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);
                        break;
                    case Codes.AccountMediator.Archive.ErrorArchiving:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

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
                var response = _accountMediator.Delete(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case Codes.AccountMediator.Delete.SuccessfullyDeleted:
                        UserData.Push(UserDataItemNames.DeletedVacancyId, id.ToString(CultureInfo.InvariantCulture));
                        UserData.Push(UserDataItemNames.DeletedVacancyTitle, response.Message.Text);
                        break;
                    case Codes.AccountMediator.Delete.AlreadyDeleted:
                    case Codes.AccountMediator.Delete.ErrorDeleting:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }
    }
}
