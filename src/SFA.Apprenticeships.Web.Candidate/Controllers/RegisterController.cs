namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Providers;
    using Constants.ViewModels;
    using Domain.Entities.Candidates;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
    using Infrastructure.Azure.Session;
    using NLog;
    using Providers;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : SfaControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(
            ISessionState session,
            IUserServiceProvider userServiceProvider,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider)
            : base(session, userServiceProvider)
        {
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
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

            var candidate = _candidateServiceProvider.Register(model);

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

            var activatedResult = _activationViewModelServerValidator.Validate(model);

            if (activatedResult.IsValid)
            {
                UserServiceProvider.SetAuthenticationCookie(
                    HttpContext, User.Identity.Name, "Activated");

                // Redirect to last viewed vacancy (if any).
                var lastViewedVacancyId = _candidateServiceProvider.LastViewedVacancyId;

                if (lastViewedVacancyId.HasValue)
                {
                    _candidateServiceProvider.LastViewedVacancyId = null;

                    return RedirectToAction("Details", "VacancySearch", new { id = lastViewedVacancyId.Value });
                }

                // Redirect to return URL (if any).
                var returnUrl = UserServiceProvider.GetAuthenticationReturnUrl(HttpContext);

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

        public JsonResult CheckUsername(CheckUsernameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {Result = false}, JsonRequestBehavior.AllowGet);
            }

            var usernameIsAvailable = IsUsernameAvailable(model.Email);

            return Json(
                new
                {
                    Result = usernameIsAvailable
                },
                JsonRequestBehavior.AllowGet);
        }

        private bool IsUsernameAvailable(string username)
        {
            return _candidateServiceProvider.IsUsernameAvailable(username.Trim()); // TODO Consider doing this everywhere
        }
        private void SetCookies(Candidate candidate)
        {
            var registrationDetails = candidate.RegistrationDetails;

            UserServiceProvider.SetAuthenticationCookie(
                HttpContext, candidate.EntityId.ToString(), "Unactivated");

            UserServiceProvider.SetUserContextCookie(
                HttpContext, registrationDetails.EmailAddress, registrationDetails.FullName);
        }
    }
}
