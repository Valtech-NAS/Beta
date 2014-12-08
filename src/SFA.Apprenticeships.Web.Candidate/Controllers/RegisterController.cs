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
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : CandidateControllerBase
    {
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly ICandidateServiceProvider _candidateServiceProvider;

        private readonly ForgottenPasswordViewModelServerValidator _forgottenPasswordViewModelServerValidator;
        private readonly PasswordResetViewModelServerValidator _passwordResetViewModelServerValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService authenticationTicketService,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            ForgottenPasswordViewModelServerValidator forgottenPasswordViewModelServerValidator,
            PasswordResetViewModelServerValidator passwordResetViewModelServerValidator)
        {
            _authenticationTicketService = authenticationTicketService;
            _candidateServiceProvider = candidateServiceProvider;
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _forgottenPasswordViewModelServerValidator = forgottenPasswordViewModelServerValidator;
            _passwordResetViewModelServerValidator = passwordResetViewModelServerValidator;
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
                var userNameAvailable = _candidateServiceProvider.IsUsernameAvailable(model.EmailAddress.Trim());
                if (!userNameAvailable.HasError)
                {
                    model.IsUsernameAvailable = userNameAvailable.IsUserNameAvailable;
                    var validationResult = _registerViewModelServerValidator.Validate(model);

                    if (!validationResult.IsValid)
                    {
                        ModelState.Clear();
                        validationResult.AddToModelState(ModelState, string.Empty);

                        return View(model);
                    }

                    var serverError = !_candidateServiceProvider.Register(model);
                    if (!serverError)
                    {
                        UserData.SetUserContext(model.EmailAddress, model.Firstname + " " + model.Lastname);

                        return RedirectToAction("Activation");
                    }
                }

                SetUserMessage(RegisterPageMessages.RegistrationFailed, UserMessageLevel.Warning);

                ModelState.Clear();
                return View(model);
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
                var model = new ActivationViewModel
                {
                    EmailAddress = UserContext.UserName
                };

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
                model = _candidateServiceProvider.Activate(model, UserContext.CandidateId);

                switch (model.State)
                {
                    case ActivateUserState.Activated:
                        var candidate = _candidateServiceProvider.GetCandidate(model.EmailAddress);
                        SetUserMessage(ActivationPageMessages.AccountActivated);
                        return SetAuthenticationCookieAndRedirectToAction(candidate);

                    case ActivateUserState.Error:
                        SetUserMessage(model.ViewModelMessage, UserMessageLevel.Warning);
                        break;

                    case ActivateUserState.InvalidCode:
                        var activatedResult = _activationViewModelServerValidator.Validate(model);
                        ModelState.Clear();
                        activatedResult.AddToModelState(ModelState, string.Empty);
                        break;
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
            return await Task.Run(() =>
            {
                ViewBag.Message = UserContext.UserName;

                return View();
            });
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
                var validationResult = _forgottenPasswordViewModelServerValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    ModelState.Clear();
                    validationResult.AddToModelState(ModelState, string.Empty);

                    return View(model);
                }

                if (_candidateServiceProvider.RequestForgottenPasswordResetCode(model))
                {
                    UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                    return RedirectToAction("ResetPassword");
                }

                SetUserMessage(PasswordResetPageMessages.FailedToSendPasswordResetCode, UserMessageLevel.Warning);

                return View(model);
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

                var model = new PasswordResetViewModel
                {
                    EmailAddress = emailAddress
                };

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
                //Password Reset Code is verified in VerifyPasswordReset. Initially assume the reset code is valid as a full check requires hitting the repo.
                model.IsPasswordResetCodeValid = true;

                var validationResult = _passwordResetViewModelServerValidator.Validate(model);

                if (validationResult.IsValid)
                {
                    model = _candidateServiceProvider.VerifyPasswordReset(model);

                    if (model.HasError())
                    {
                        SetUserMessage(model.ViewModelMessage, UserMessageLevel.Warning);
                        return View(model);
                    }

                    if (model.UserStatus == UserStatuses.Locked)
                    {
                        UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                        return RedirectToAction("Unlock", "Login");
                    }

                    validationResult = _passwordResetViewModelServerValidator.Validate(model);
                    if (validationResult.IsValid)
                    {
                        var candidate = _candidateServiceProvider.GetCandidate(model.EmailAddress);

                        SetUserMessage(PasswordResetPageMessages.SuccessfulPasswordReset);

                        return SetAuthenticationCookieAndRedirectToAction(candidate);
                    }
                }

                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
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
                var model = new ForgottenPasswordViewModel
                {
                    EmailAddress = emailAddress
                };

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

        private ActionResult SetAuthenticationCookieAndRedirectToAction(Candidate candidate)
        {
            //todo: refactor - similar to stuff in login controller... move to ILoginServiceProvider
            //todo: test this
            _authenticationTicketService.SetAuthenticationCookie(HttpContext.Response.Cookies,
                candidate.EntityId.ToString(), UserRoleNames.Activated);
            UserData.SetUserContext(candidate.RegistrationDetails.EmailAddress,
                candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName);

            // ReturnUrl takes precedence over last view vacnacy id.
            var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

            // Clear last viewed vacancy and distance (if any).
            var lastViewedVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);
            UserData.Pop(UserDataItemNames.VacancyDistance);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(Server.UrlDecode(returnUrl));
            }

            if (lastViewedVacancyId != null)
            {
                return RedirectToRoute(CandidateRouteNames.Details, new {id = int.Parse(lastViewedVacancyId)});
            }

            return RedirectToRoute(CandidateRouteNames.Search);
        }

        #endregion
    }
}
