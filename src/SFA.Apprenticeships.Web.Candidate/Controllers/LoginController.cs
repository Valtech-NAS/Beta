namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Providers;
    using Constants.ViewModels;
    using Domain.Entities.Candidates;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginController : SfaControllerBase
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider,
            LoginViewModelServerValidator loginViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider)
            : base(session, userServiceProvider)
        {
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
        }

        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserServiceProvider.SetAuthenticationReturnUrlCookie(HttpContext, returnUrl);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            // Validate view model.
            var validationResult = _loginViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            // Attempt to authenticate candidate.
            var candidate = _candidateServiceProvider.Authenticate(model);

            if (candidate == null)
            {
                // Authentication failed.
                ModelState.Clear();
                ModelState.AddModelError(
                    "EmailAddress",
                    LoginViewModelMessages.AuthenticationMessages.AuthenticationFailedErrorText);

                return View(model);
            }

            SetCookies(candidate);

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

        private void SetCookies(Candidate candidate)
        {
            var registrationDetails = candidate.RegistrationDetails;
            var roles = _candidateServiceProvider.GetRoles(registrationDetails.EmailAddress);

            string fullName = registrationDetails.FirstName + " " + registrationDetails.LastName;

            UserServiceProvider.SetAuthenticationCookie(
                HttpContext, candidate.EntityId.ToString(), roles);

            UserServiceProvider.SetUserContextCookie(
                HttpContext, registrationDetails.EmailAddress, fullName);
        }
    }
}
