namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Constants.ViewModels;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
    using Infrastructure.Azure.Session;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginController : SfaControllerBase
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginController(
            ISessionState session,
            LoginViewModelServerValidator loginViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider)
            : base(session)
        {
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginView)
        {
            // Validate view model.
            ValidationResult validationResult = _loginViewModelServerValidator.Validate(loginView);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(loginView);
            }

            // Attempt to authenticate candidate.
            bool authenticated = _candidateServiceProvider.Authenticate(loginView);

            if (authenticated)
            {
                return RedirectToAction("Index", "VacancySearch");
            }

            // Authentication failed.
            ModelState.Clear();
            ModelState.AddModelError(
                "EmailAddress",
                LoginViewModelMessages.AuthenticationMessages.AuthenticationFailedErrorText);

            return View(loginView);
        }
    }
}
