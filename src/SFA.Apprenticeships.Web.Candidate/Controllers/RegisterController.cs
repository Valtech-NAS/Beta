namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Register;
    using Providers;
    using ViewModels.Register;

    public class RegisterController : CandidateControllerBase
    {
        private readonly IRegisterMediator _registerMediator;
        private readonly IConfigurationManager _configurationManager;

        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly ICandidateServiceProvider _candidateServiceProvider;

        public RegisterController(ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService authenticationTicketService,
            IRegisterMediator registerMediator,
            IConfigurationManager configurationManager)
        {
            _authenticationTicketService = authenticationTicketService;
            _candidateServiceProvider = candidateServiceProvider;
            _registerMediator = registerMediator;
            _configurationManager = configurationManager;
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        [AllowReturnUrl(Allow = false)]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _registerMediator.Register(model);

                ModelState.Clear();

                switch (response.Code)
                {
                    case RegisterMediatorCodes.Register.ValidationFailed:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case RegisterMediatorCodes.Register.RegistrationFailed:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(model);
                    case RegisterMediatorCodes.Register.SuccessfullyRegistered:
                        UserData.SetUserContext(model.EmailAddress, model.Firstname + " " + model.Lastname, _configurationManager.GetAppSetting<string>(Settings.TermsAndConditionsVersion));
                        return RedirectToAction("Activation");
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Unactivated)]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> Activation(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = new ActivationViewModel { EmailAddress = UserContext.UserName };

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    UserData.Push(UserDataItemNames.ReturnUrl, Server.UrlEncode(returnUrl));
                }

                return View(model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Unactivated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Activate(ActivationViewModel model)
        {
            return await Task.Run(() =>
            {
                var response = _registerMediator.Activate(UserContext.CandidateId, model);

                switch (response.Code)
                {
                    case RegisterMediatorCodes.Activate.SuccessfullyActivated:
                        SetUserMessage(response.Message.Text);
                        return SetAuthenticationCookieAndRedirectToAction(model.EmailAddress);
                    case RegisterMediatorCodes.Activate.InvalidActivationCode:
                    case RegisterMediatorCodes.Activate.FailedValidation:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        break;
                    case RegisterMediatorCodes.Activate.ErrorActivating:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return View("Activation", model);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Complete()
        {
            return await Task.Run(() => View(UserContext.UserName));
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> ForgottenPassword()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> ForgottenPassword(ForgottenPasswordViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _registerMediator.ForgottenPassword(model);

                ModelState.Clear();

                switch (response.Code)
                {
                    case RegisterMediatorCodes.ForgottenPassword.FailedValidation:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case RegisterMediatorCodes.ForgottenPassword.FailedToSendResetCode:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case RegisterMediatorCodes.ForgottenPassword.PasswordSent:
                        UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                        return RedirectToAction("ResetPassword");
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> ResetPassword()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var emailAddress = UserData.Get(UserDataItemNames.EmailAddress);

                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    return RedirectToAction("ForgottenPassword");
                }

                var model = new PasswordResetViewModel { EmailAddress = emailAddress };
                return View(model);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        [ValidateInput(false)]
        public async Task<ActionResult> ResetPassword(PasswordResetViewModel model)
        {
            return await Task.Run(() =>
            {
                var response = _registerMediator.ResetPassword(model);

                switch (response.Code)
                {
                    case RegisterMediatorCodes.ResetPassword.FailedValidation:
                    case RegisterMediatorCodes.ResetPassword.InvalidResetCode:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    
                    case RegisterMediatorCodes.ResetPassword.FailedToResetPassword:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(model);

                    case RegisterMediatorCodes.ResetPassword.UserAccountLocked:
                        UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                        return RedirectToAction("Unlock", "Login");

                    case RegisterMediatorCodes.ResetPassword.SuccessfullyResetPassword:
                        SetUserMessage(response.Message.Text);
                        return SetAuthenticationCookieAndRedirectToAction(model.EmailAddress);

                    default:
                        throw new InvalidMediatorCodeException(response.Code);

                }
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> ResendPasswordResetCode(string emailAddress)
        {
            return await Task.Run(() =>
            {
                var model = new ForgottenPasswordViewModel { EmailAddress = emailAddress };

                UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);

                if (_candidateServiceProvider.RequestForgottenPasswordResetCode(model))
                {
                    SetUserMessage(string.Format(PasswordResetPageMessages.PasswordResetSent, emailAddress));
                }
                else
                {
                    SetUserMessage(PasswordResetPageMessages.FailedToSendPasswordResetCode, UserMessageLevel.Warning);
                }

                return RedirectToAction("ResetPassword");
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [ApplyWebTrends]
        public async Task<ActionResult> ResendActivationCode(string emailAddress)
        {
            return await Task.Run(() =>
            {
                if (_candidateServiceProvider.ResendActivationCode(emailAddress))
                {
                    SetUserMessage(string.Format(ActivationPageMessages.ActivationCodeSent, emailAddress));

                    return RedirectToAction("Activation");
                }

                SetUserMessage(ActivationPageMessages.ActivationCodeSendingFailure, UserMessageLevel.Warning);
                return RedirectToAction("Activation");
            });
        }

        [AllowCrossSiteJson]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public async Task<ActionResult> CheckUsername(string username)
        {
            return await Task.Run(() =>
            {
                var userNameAvailability = _candidateServiceProvider.IsUsernameAvailable(username.Trim());

                return Json(userNameAvailability, JsonRequestBehavior.AllowGet);
            });
        }

        #region Helpers

        private ActionResult SetAuthenticationCookieAndRedirectToAction(string candidateEmail)
        {
            var candidate = _candidateServiceProvider.GetCandidate(candidateEmail);
            //todo: refactor - similar to stuff in login controller... move to ILoginServiceProvider
            //todo: test this
            _authenticationTicketService.SetAuthenticationCookie(HttpContext.Response.Cookies, candidate.EntityId.ToString(), UserRoleNames.Activated);
            UserData.SetUserContext(candidate.RegistrationDetails.EmailAddress,
                candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName,
                candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion);

            // ReturnUrl takes precedence over last view vacnacy id.
            var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

            // Clear last viewed vacancy and distance (if any).
            var lastViewedVacancyId = UserData.Pop(CandidateDataItemNames.LastViewedVacancyId);
            UserData.Pop(CandidateDataItemNames.VacancyDistance);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(Server.UrlDecode(returnUrl));
            }

            if (lastViewedVacancyId != null)
            {
                return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new {id = int.Parse(lastViewedVacancyId)});
            }

            return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
        }

        #endregion
    }
}
