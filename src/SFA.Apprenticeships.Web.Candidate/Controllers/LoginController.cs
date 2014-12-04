namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginController : CandidateControllerBase
    {
        private readonly AccountUnlockViewModelServerValidator _accountUnlockViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginController(LoginViewModelServerValidator loginViewModelServerValidator,
            AccountUnlockViewModelServerValidator accountUnlockViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService authenticationTicketService)
        {
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _accountUnlockViewModelServerValidator = accountUnlockViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
            _authenticationTicketService = authenticationTicketService; //todo: shouldn't be in here, move to Provider layer
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                _authenticationTicketService.Clear(HttpContext.Request.Cookies);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    UserData.Push(UserDataItemNames.ReturnUrl, Server.UrlEncode(returnUrl));
                }

                return View();
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            return await Task.Run(() =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                // todo: refactor - too much going on here Provider layer
                // Validate view model.
                var validationResult = _loginViewModelServerValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    ModelState.Clear();
                    validationResult.AddToModelState(ModelState, string.Empty);

                    return View(model);
                }

                var result = _candidateServiceProvider.Login(model);

                if (result.UserStatus == UserStatuses.Locked)
                {
                    UserData.Push(UserDataItemNames.EmailAddress, result.EmailAddress);

                    return RedirectToAction("Unlock");
                }

                if (result.IsAuthenticated)
                {
                    UserData.SetUserContext(result.EmailAddress, result.FullName);

                    return RedirectOnAuthenticated(result.UserStatus, result.EmailAddress);
                }

                ModelState.Clear();
                ModelState.AddModelError(string.Empty, result.ViewModelMessage);

                return View(model);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> Unlock()
        {
            return await Task.Run(() =>
            {
                //TODO: Should be blank UNLESS you have logged in with valid credentials but we have detected that your account is locked or at the moment when you have locked your account
                var emailAddress = UserData.Get(UserDataItemNames.EmailAddress);

                return ViewAccountUnlock(emailAddress);
            });
        }


        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Unlock(AccountUnlockViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                // todo: refactor - too much going on here Provider layer
                var validationResult = _accountUnlockViewModelServerValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    ModelState.Clear();
                    validationResult.AddToModelState(ModelState, string.Empty);

                    return View(model);
                }

                var accountUnlockViewModel = _candidateServiceProvider.VerifyAccountUnlockCode(model);
                switch (accountUnlockViewModel.Status)
                {
                    case AccountUnlockState.Ok:
                        UserData.Pop(UserDataItemNames.EmailAddress);
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockedText);
                        return RedirectToAction("Index");

                    case AccountUnlockState.UserInIncorrectState:
                        return RedirectToAction("Index");
                    case AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid:
                        SetUserMessage(AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error);
                        return View(model);

                    case AccountUnlockState.AccountUnlockCodeExpired:
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                        return View(model);

                    default:
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
                        return View(model);
                }
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> ResendAccountUnlockCode(string emailAddress)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = new AccountUnlockViewModel
                {
                    EmailAddress = emailAddress
                };

                //TODO: make different things if we have an error or a user with an invalid state

                model = _candidateServiceProvider.RequestAccountUnlockCode(model);
                UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);

                if (model.HasError())
                {
                    SetUserMessage(AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning);
                }
                else
                {
                    SetUserMessage(string.Format(AccountUnlockPageMessages.AccountUnlockCodeResent, emailAddress));
                }

                return RedirectToAction("Unlock");
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            UserData.Clear();

            SetUserMessage(SignOutPageMessages.SignOutMessageText);

            return RedirectToAction("Index");
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public ActionResult SessionTimeout(string returnUrl)
        {

            FormsAuthentication.SignOut();
            UserData.Clear();

            SetUserMessage(SignOutPageMessages.SessionTimeoutMessageText);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserData.Push(UserDataItemNames.SessionReturnUrl, Server.UrlEncode(returnUrl));
            }

            return RedirectToAction("Index");
        }

        #region Helpers

        private ActionResult ViewAccountUnlock(string emailAddress)
        {
            return View(new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            });
        }

        private ActionResult RedirectOnAuthenticated(UserStatuses userStatus, string username)
        {
            // todo: refactor - too much going on here Provider layer
            if (userStatus == UserStatuses.PendingActivation)
            {
                return RedirectToAction("Activation", "Register");
            }

            // Redirect to session return URL (if any).
            var sessionReturnUrl = UserData.Pop(UserDataItemNames.SessionReturnUrl);

            if (!string.IsNullOrWhiteSpace(sessionReturnUrl))
            {
                return Redirect(Server.UrlDecode(sessionReturnUrl));
            }

            // Redirect to return URL (if any).
            var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(Server.UrlDecode(returnUrl));
            }

            // Redirect to last viewed vacancy (if any).
            var lastViewedVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);

            if (lastViewedVacancyId != null)
            {
                var candidate = _candidateServiceProvider.GetCandidate(username);

                var applicationStatus = _candidateServiceProvider.GetApplicationStatus(
                    candidate.EntityId, int.Parse(lastViewedVacancyId));

                if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
                {
                    return RedirectToAction("Apply", "Application", new { id = lastViewedVacancyId });
                }

                return RedirectToRoute(CandidateRouteNames.Details, new { id = lastViewedVacancyId });
            }

            return RedirectToRoute(CandidateRouteNames.MyApplications);
        }

        #endregion
    }
}
