namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Services;
    using Constants.ViewModels;
    using Domain.Entities.Candidates;
    using FluentValidation.Mvc;
    using Infrastructure.Azure.Session;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginController : SfaControllerBase
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IAuthenticationTicketService _ticketService;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginController(
            ISessionState session,
            LoginViewModelServerValidator loginViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService ticketService)
            : base(session)
        {
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
            _ticketService = ticketService;
        }

        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                TempData["ReturnUrl"] = returnUrl;
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

            AddCookies(candidate);

            var returnUrl = TempData["ReturnUrl"] as string;

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "VacancySearch");
        }

        private void AddCookies(Candidate candidate)
        {
            var roles = _candidateServiceProvider.GetRoles(candidate.RegistrationDetails.EmailAddress);
            var ticket = _ticketService.CreateTicket(candidate.EntityId.ToString(), roles);

            _ticketService.AddTicket(Response.Cookies, ticket);

            var cookie = CreateContextCookie(candidate.RegistrationDetails);

            Response.Cookies.Add(cookie);
        }

        private static HttpCookie CreateContextCookie(RegistrationDetails registrationDetails)
        {
            var cookie = new HttpCookie("User.Context");

            cookie.Values.Add("UserName", registrationDetails.EmailAddress);
            cookie.Values.Add("FullName", registrationDetails.FirstName + " " + registrationDetails.LastName);

            return cookie;
        }
    }
}
