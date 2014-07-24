namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Constants.ViewModels;
    using Domain.Entities.Candidates;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
    using NLog;
    using Providers;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : SfaControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ForgottenPasswordViewModelServerValidator _forgottenPasswordViewModelServerValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider,
            ICandidateServiceProvider candidateServiceProvider,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            ForgottenPasswordViewModelServerValidator forgottenPasswordViewModelServerValidator)
            : base(session, userServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _forgottenPasswordViewModelServerValidator = forgottenPasswordViewModelServerValidator;
        }

        public ActionResult Index()
        {
            Logger.Debug("Someone trying to register");
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel model)
        {
            model.IsUsernameAvailable = IsUsernameAvailable(model.EmailAddress);

            ValidationResult validationResult = _registerViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            Candidate candidate = _candidateServiceProvider.Register(model);

            if (candidate == null)
            {
                // TODO: Registration failed.
                ModelState.Clear();
                ModelState.AddModelError(
                    "EmailAddress",
                    RegisterViewModelMessages.RegistrationMessages.RegistrationFailedErrorText);

                return View(model);
            }

            SetCookies(candidate);

            return RedirectToAction("Activation");
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Unactivated)]
        public ActionResult Activation(string returnUrl)
        {
            var model = new ActivationViewModel
            {
                EmailAddress = UserContext.UserName
            };

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserServiceProvider.SetAuthenticationReturnUrlCookie(HttpContext, returnUrl);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Activate(ActivationViewModel model)
        {
            model.IsActivated = _candidateServiceProvider.Activate(model);

            ValidationResult activatedResult = _activationViewModelServerValidator.Validate(model);

            if (activatedResult.IsValid)
            {
                UserServiceProvider.SetAuthenticationCookie(
                    HttpContext, User.Identity.Name, UserRoleNames.Activated);

                // Redirect to last viewed vacancy (if any).
                int? lastViewedVacancyId = _candidateServiceProvider.LastViewedVacancyId;

                if (lastViewedVacancyId.HasValue)
                {
                    _candidateServiceProvider.LastViewedVacancyId = null;

                    return RedirectToAction("Details", "VacancySearch", new {id = lastViewedVacancyId.Value});
                }

                // Redirect to return URL (if any).
                string returnUrl = UserServiceProvider.GetAuthenticationReturnUrl(HttpContext);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    UserServiceProvider.DeleteAuthenticationReturnUrlCookie(HttpContext);

                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "VacancySearch");
            }

            ModelState.Clear();
            activatedResult.AddToModelState(ModelState, string.Empty);

            return View("Activation", model);
        }

        public ActionResult Complete()
        {
            ViewBag.Message = UserContext.UserName;

            return View();
        }

        [HttpGet]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgottenPassword(ForgottenPasswordViewModel model)
        {
            ValidationResult validationResult = _forgottenPasswordViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            return RedirectToAction("EnterPasswordResetCode");
        }

        public ActionResult EnterPasswordResetCode()
        {
            return View();
        } 
        
        [HttpPost]
        public ActionResult EnterPasswordResetCode(EnterPasswordResetCodeViewModel model)
        {
            return View(model);
        }

        public ActionResult ResetPassword()
        {
            return View();
        } 
        
        [HttpPost]
        public ActionResult ResetPassword(PasswordResetViewModel model)
        {
            return View(model);
        }

        #region Helpers
	[AllowCrossSiteJson]
        public JsonResult CheckUsername(string username)
        {
            bool usernameIsAvailable = IsUsernameAvailable(username);
            return Json(new {usernameIsAvailable}, JsonRequestBehavior.AllowGet);
        }

        private bool IsUsernameAvailable(string username)
        {
            return _candidateServiceProvider.IsUsernameAvailable(username.Trim());
            // TODO Consider doing this everywhere
        }

        private void SetCookies(Candidate candidate)
        {
            RegistrationDetails registrationDetails = candidate.RegistrationDetails;

            UserServiceProvider.SetAuthenticationCookie(
                HttpContext, candidate.EntityId.ToString(), UserRoleNames.Unactivated);

            UserServiceProvider.SetUserContextCookie(
                HttpContext, registrationDetails.EmailAddress, registrationDetails.FullName);
        }

        #endregion
    }
}