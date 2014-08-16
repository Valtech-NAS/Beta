namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Providers;
    using Common.Services;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : CandidateControllerBase
    {
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ForgottenPasswordViewModelServerValidator _forgottenPasswordViewModelServerValidator;
        private readonly PasswordResetViewModelServerValidator _passwordResetViewModelServerValidator;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(
            ISessionStateProvider session,
            ICandidateServiceProvider candidateServiceProvider,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            ForgottenPasswordViewModelServerValidator forgottenPasswordViewModelServerValidator,
            PasswordResetViewModelServerValidator passwordResetViewModelServerValidator,
            IAuthenticationTicketService authenticationTicketService)
            : base(session)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _forgottenPasswordViewModelServerValidator = forgottenPasswordViewModelServerValidator;
            _passwordResetViewModelServerValidator = passwordResetViewModelServerValidator;
            _authenticationTicketService = authenticationTicketService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel model)
        {
            model.IsUsernameAvailable = _candidateServiceProvider.IsUsernameAvailable(model.EmailAddress.Trim());

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
                ModelState.Clear();
                ModelState.AddModelError("EmailAddress", RegisterPageMessages.RegistrationFailed);

                return View(model);
            }

            UserData.SetUserContext(model.EmailAddress, model.Firstname + " " + model.Lastname);

            return RedirectToAction("Activation");
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
                UserData.Push(UserDataItemNames.ReturnUrl, returnUrl);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Activate(ActivationViewModel model)
        {
            //todo: refactor - too much going on here
            model.IsActivated = _candidateServiceProvider.Activate(model, UserContext.CandidateId);

            var activatedResult = _activationViewModelServerValidator.Validate(model);

            if (activatedResult.IsValid)
            {
                var candidate = _candidateServiceProvider.GetCandidate(model.EmailAddress);

                return SetAuthenticationCookieAndRedirectToAction(candidate);
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
            var validationResult = _forgottenPasswordViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            _candidateServiceProvider.RequestForgottenPasswordResetCode(model);

            UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);

            return RedirectToAction("ResetPassword");
        }

        [HttpGet]
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
                        UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);
                        return RedirectToAction("Unlock", "Login");
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

            var validationResult = _passwordResetViewModelServerValidator.Validate(model);

            if (validationResult.IsValid)
            {
                var candidate = _candidateServiceProvider.GetCandidate(model.EmailAddress);

                return SetAuthenticationCookieAndRedirectToAction(candidate);
            }

            ModelState.Clear();
            validationResult.AddToModelState(ModelState, string.Empty);

            return View(model);
        }

        [HttpGet]
        public ActionResult ResendPasswordResetCode(string emailAddress)
        {
            var model = new ForgottenPasswordViewModel
            {
                EmailAddress = emailAddress
            };

            _candidateServiceProvider.RequestForgottenPasswordResetCode(model);

            UserData.Push(UserDataItemNames.EmailAddress, model.EmailAddress);

            SetUserMessage(string.Format(PasswordResetPageMessages.PasswordResetSent, emailAddress));

            return RedirectToAction("ResetPassword");
        }

        public ActionResult ResendActivationCode(string emailAddress)
        {
            _candidateServiceProvider.ResendActivationCode(emailAddress);

            SetUserMessage(string.Format(ActivationPageMessages.ActivationCodeSent, emailAddress));

            return RedirectToAction("Activation");
        }

        [AllowCrossSiteJson]
        public JsonResult CheckUsername(string username)
        {
            var isUsernameAvailable = _candidateServiceProvider.IsUsernameAvailable(username.Trim());

            return Json(new { isUsernameAvailable }, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private ActionResult SetAuthenticationCookieAndRedirectToAction(Candidate candidate)
        {
            //todo: refactor - similar to stuff in login controller... move to ILoginServiceProvider
            //todo: test this
            _authenticationTicketService.SetAuthenticationCookie(HttpContext.Response.Cookies, candidate.EntityId.ToString(), UserRoleNames.Activated);
            UserData.SetUserContext(candidate.RegistrationDetails.EmailAddress, candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName);

            // Redirect to last viewed vacancy (if any).
            var lastViewedVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);

            if (lastViewedVacancyId != null)
            {
                return RedirectToAction("Details", "VacancySearch", new {id = int.Parse(lastViewedVacancyId)});
            }

            // Redirect to return URL (if any).
            var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "VacancySearch");
            }

            return Redirect(returnUrl);
        }

        #endregion
    }
}
