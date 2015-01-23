namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Mediators;
    using ViewModels.Login;

    public class LoginController : CandidateControllerBase
    {
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly ILoginMediator _loginMediator;
     
        public LoginController(IAuthenticationTicketService authenticationTicketService,
            ILoginMediator loginMediator)
        {
            _authenticationTicketService = authenticationTicketService; //todo: shouldn't be in here, move to Provider layer?
            _loginMediator = loginMediator;
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
            return await Task.Run<ActionResult>(() =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToRoute(CandidateRouteNames.MyApplications);
                }

                var response = _loginMediator.Index(model);

                switch (response.Code)
                {
                    case Codes.Login.Index.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);

                    case Codes.Login.Index.AccountLocked:
                        return RedirectToAction("Unlock");

                    case Codes.Login.Index.ApprenticeshipApply:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipApply, new { id = response.Parameters.ToString() });

                    case Codes.Login.Index.ApprenticeshipDetails:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id = response.Parameters.ToString() });

                    case Codes.Login.Index.ReturnUrl:
                        return Redirect(HttpUtility.UrlDecode(response.Parameters.ToString()));

                    case Codes.Login.Index.Ok:
                        return RedirectToRoute(CandidateRouteNames.MyApplications);

                    case Codes.Login.Index.PendingActivation:
                        return RedirectToAction("Activation", "Register");

                    case Codes.Login.Index.LoginFailed:
                        ModelState.AddModelError(string.Empty, response.Parameters.ToString());
                        return View(model);

                    case Codes.Login.Index.TermsAndConditionsNeedAccepted:
                        if (response.Parameters != null)
                        {
                            var returnUrl = new { ReturnUrl = HttpUtility.UrlDecode(response.Parameters.ToString())};
                            return RedirectToRoute(RouteNames.UpdatedTermsAndConditions, returnUrl);
                        }
                        return RedirectToRoute(RouteNames.UpdatedTermsAndConditions);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
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
                var emailAddress = UserData.Pop(UserDataItemNames.UnlockEmailAddress);
                return View(new AccountUnlockViewModel { EmailAddress = emailAddress });
            });
        }


        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [MultipleFormActionsButton(Name = "LoginAction", Argument = "Unlock")]
        [ApplyWebTrends]
        public async Task<ActionResult> Unlock(AccountUnlockViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _loginMediator.Unlock(model);

                switch (response.Code)
                {
                    case Codes.Login.Unlock.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);

                    case Codes.Login.Unlock.UnlockedSuccessfully:
                        UserData.Pop(UserDataItemNames.EmailAddress);
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockedText);
                        return RedirectToRoute(RouteNames.SignIn);

                    case Codes.Login.Unlock.UserInIncorrectState:
                        return RedirectToRoute(RouteNames.SignIn);

                    case Codes.Login.Unlock.AccountEmailAddressOrUnlockCodeInvalid:
                        SetUserMessage(AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error);
                        return View(model);

                    case Codes.Login.Unlock.AccountUnlockCodeExpired:
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                        return View(model);

                    case Codes.Login.Unlock.AccountUnlockFailed:
                        SetUserMessage(AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
                        return View(model);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [MultipleFormActionsButton(Name = "LoginAction", Argument = "Resend")]
        [ApplyWebTrends]
        public async Task<ActionResult> Resend(AccountUnlockViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _loginMediator.Resend(model);

                switch (response.Code)
                {
                    case Codes.Login.Resend.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);

                    case Codes.Login.Resend.ResendFailed:
                    case Codes.Login.Resend.ResentSuccessfully:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return RedirectToAction("Unlock");

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public ActionResult SignOut(string returnUrl)
        {
            FormsAuthentication.SignOut();

            if (UserData.Get(UserMessageConstants.WarningMessage) == SignOutPageMessages.MustAcceptUpdatedTermsAndConditions)
            {
                UserData.Clear();
                SetUserMessage(SignOutPageMessages.MustAcceptUpdatedTermsAndConditions, UserMessageLevel.Warning);
            }
            else
            {
                UserData.Clear();
                SetUserMessage(SignOutPageMessages.SignOutMessageText);
            }

            return !string.IsNullOrEmpty(returnUrl)
                ? RedirectToRoute(RouteNames.SignIn, new {ReturnUrl = returnUrl})
                : RedirectToRoute(RouteNames.SignIn);
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public ActionResult SessionTimeout(string returnUrl)
        {
            var userContext = UserData.GetUserContext();

            FormsAuthentication.SignOut();
            UserData.Clear();
            
            if (userContext != null)
            {
                //Only set the message if the user context was set by a previous login action.
                //This means that the session has timed out rather than becoming invalid after closing the browser.
                SetUserMessage(SignOutPageMessages.SessionTimeoutMessageText);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserData.Push(UserDataItemNames.SessionReturnUrl, Server.UrlEncode(returnUrl));
            }

            return RedirectToRoute(RouteNames.SignIn);
        }
    }
}
