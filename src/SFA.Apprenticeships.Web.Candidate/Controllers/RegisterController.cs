namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Services;
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

        [OutputCache(CacheProfile = "Long")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel model)
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
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Unactivated)]
        [NoCache]
        public ActionResult Activation(string returnUrl)
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
        }

        [HttpPost]
        public ActionResult Activate(ActivationViewModel model)
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
        }

        public ActionResult Complete()
        {
            ViewBag.Message = UserContext.UserName;

            return View();
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [OutputCache(CacheProfile = "Long")]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgottenPassword(ForgottenPasswordViewModel model)
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
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        public ActionResult ResetPassword()
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
        }

        [HttpPost]
        public ActionResult ResetPassword(PasswordResetViewModel model)
        {
            var result = _candidateServiceProvider.VerifyPasswordReset(model);

            if (result.HasError())
            {
                SetUserMessage(result.ViewModelMessage, UserMessageLevel.Warning);
                return View(result);
            }

            if (result.UserStatus == UserStatuses.Locked)
            {
                UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                return RedirectToAction("Unlock", "Login");
            }

            var validationResult = _passwordResetViewModelServerValidator.Validate(result);

            if (validationResult.IsValid)
            {
                var candidate = _candidateServiceProvider.GetCandidate(result.EmailAddress);

                SetUserMessage(PasswordResetPageMessages.SuccessfulPasswordReset);

                return SetAuthenticationCookieAndRedirectToAction(candidate);
            }

            ModelState.Clear();
            validationResult.AddToModelState(ModelState, string.Empty);

            return View(result);
        }

        [HttpGet]
        public ActionResult ResendPasswordResetCode(string emailAddress)
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
        }

        public ActionResult ResendActivationCode(string emailAddress)
        {
            if (_candidateServiceProvider.ResendActivationCode(emailAddress))
            {
                SetUserMessage(string.Format(ActivationPageMessages.ActivationCodeSent, emailAddress));

                return RedirectToAction("Activation");
            }

            SetUserMessage(ActivationPageMessages.ActivationCodeSendingFailure, UserMessageLevel.Warning);
            return RedirectToAction("Activation");
        }

        [AllowCrossSiteJson]
        public ActionResult CheckUsername(string username)
        {
            var userNameAvailability = _candidateServiceProvider.IsUsernameAvailable(username.Trim());

            return Json(userNameAvailability, JsonRequestBehavior.AllowGet);
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
                return RedirectToAction("Details", "VacancySearch", new {id = int.Parse(lastViewedVacancyId)});
            }

            return RedirectToAction("Index", "VacancySearch");
        }

        #endregion
    }
}
