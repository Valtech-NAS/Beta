namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Constants.ViewModels;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
    using NLog;
    using Providers;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : SfaControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ForgottenPasswordViewModelServerValidator _forgottenPasswordViewModelServerValidator;
        private readonly PasswordResetViewModelServerValidator _passwordResetViewModelServerValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;

        public RegisterController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider,
            ICandidateServiceProvider candidateServiceProvider,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            ForgottenPasswordViewModelServerValidator forgottenPasswordViewModelServerValidator,
            PasswordResetViewModelServerValidator passwordResetViewModelServerValidator)
            : base(session, userServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _forgottenPasswordViewModelServerValidator = forgottenPasswordViewModelServerValidator;
            _passwordResetViewModelServerValidator = passwordResetViewModelServerValidator;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel model)
        {
            model.IsUsernameAvailable = IsUsernameAvailable(model.EmailAddress);

            var validationResult = _registerViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            var registered = _candidateServiceProvider.Register(model);

            if (!registered)
            {
                // TODO: Registration failed.
                ModelState.Clear();
                ModelState.AddModelError(
                    "EmailAddress",
                    RegisterViewModelMessages.RegistrationMessages.RegistrationFailedErrorText);

                return View(model);
            }

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
                UserServiceProvider.SetReturnUrlCookie(HttpContext, returnUrl);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Activate(ActivationViewModel model)
        {
            model.IsActivated = _candidateServiceProvider.Activate(model, User.Identity.Name);

            ValidationResult activatedResult = _activationViewModelServerValidator.Validate(model);

            if (activatedResult.IsValid)
            {
                return SetAuthenticationCookieAndRedirectToAction();
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

            Logger.Debug("{0} requested password reset code", model.EmailAddress);

            _candidateServiceProvider.RequestForgottenPasswordReset(model);

            TempData["ForgottenPasswordEmailAddress"] = model.EmailAddress;

            return RedirectToAction("ResetPassword");
        }

        public ActionResult ResetPassword()
        {
            var model = new PasswordResetViewModel
            {
                EmailAddress = TempData["ForgottenPasswordEmailAddress"].ToString()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetPassword(PasswordResetViewModel model)
        {
            try
            {
                _candidateServiceProvider.VerifyPasswordReset(model);
                model.IsPasswordResetSuccessful = true;
                model.IsPasswordResetCodeInvalid = true;
            }
            catch (CustomException exception)
            {
                switch (exception.Code)
                {
                    case ErrorCodes.UnknownUserError:
                        model.IsPasswordResetSuccessful = false;
                        model.IsPasswordResetCodeInvalid = false;
                        break;
                    case ErrorCodes.UserAccountLockedError:
                        TempData["EmailAddress"] = model.EmailAddress;
                        return RedirectToAction("AccountUnlock", "Login");
                    case ErrorCodes.UserInIncorrectStateError:
                        model.IsPasswordResetCodeInvalid = false;
                        model.IsPasswordResetSuccessful = false;
                        break;
                    case ErrorCodes.UserPasswordResetCodeExpiredError:
                        model.IsPasswordResetCodeInvalid = false;
                        break;
                    case ErrorCodes.UserPasswordResetCodeIsInvalid:
                        model.IsPasswordResetCodeInvalid = false;
                        model.IsPasswordResetSuccessful = false;
                        break;
                    default:
                        model.IsPasswordResetSuccessful = false;
                        model.IsPasswordResetCodeInvalid = false;
                        break;
                }
            }

            ValidationResult validationResult = _passwordResetViewModelServerValidator.Validate(model);

            if (validationResult.IsValid)
            {
                return SetAuthenticationCookieAndRedirectToAction();
            }

            ModelState.Clear();
            validationResult.AddToModelState(ModelState, string.Empty);

            return View(model);
        }

        #region Helpers
        [AllowCrossSiteJson]
        public JsonResult CheckUsername(string username)
        {
            bool usernameIsAvailable = IsUsernameAvailable(username);
            return Json(new { usernameIsAvailable }, JsonRequestBehavior.AllowGet);
        }

        private bool IsUsernameAvailable(string username)
        {
            return _candidateServiceProvider.IsUsernameAvailable(username.Trim());
            // TODO Consider doing this everywhere
        }


        private void SetCookies(Candidate candidate)
        {
            RegistrationDetails registrationDetails = candidate.RegistrationDetails;
            string fullName = registrationDetails.FirstName + " " + registrationDetails.LastName;
            UserServiceProvider.SetAuthenticationCookie(
                HttpContext, candidate.EntityId.ToString(), UserRoleNames.Unactivated);

            UserServiceProvider.SetUserContextCookie(
                HttpContext, registrationDetails.EmailAddress, fullName);

        }

        private ActionResult SetAuthenticationCookieAndRedirectToAction()
        {
            UserServiceProvider.SetAuthenticationCookie(
                HttpContext, User.Identity.Name, UserRoleNames.Activated);

            // Redirect to last viewed vacancy (if any).
            var lastViewedVacancyId = _candidateServiceProvider.LastViewedVacancyId;

            if (lastViewedVacancyId.HasValue)
            {
                _candidateServiceProvider.LastViewedVacancyId = null;

                return RedirectToAction("Details", "VacancySearch", new { id = lastViewedVacancyId.Value });
            }

            // Redirect to return URL (if any).
            var returnUrl = UserServiceProvider.GetReturnUrl(HttpContext);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "VacancySearch");
            }

            UserServiceProvider.DeleteReturnUrlCookie(HttpContext);
            return Redirect(returnUrl);
        }

        #endregion

        public ActionResult  ResendPasswordResetCode(string emailAddress)
        {
            var model = new ForgottenPasswordViewModel
            {
                EmailAddress = emailAddress
            };

            Logger.Debug("{0} requested password reset code", model.EmailAddress);

            _candidateServiceProvider.RequestForgottenPasswordReset(model);

            TempData["ForgottenPasswordEmailAddress"] = model.EmailAddress;

            return RedirectToAction("ResetPassword");
        }
    }
}