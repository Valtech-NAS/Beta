namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Common.Controllers;
    using Common.Services;
    using FluentValidation.Mvc;
    using FluentValidation.Results;
    using Infrastructure.Azure.Session;
    using NLog;
    using Providers;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : SfaControllerBase
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;
        private readonly IAuthenticationTicketService _ticketService;

        public RegisterController(
            ISessionState session,
            RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService ticketService)
            : base(session)
        {
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
            _ticketService = ticketService;
        }

        public ActionResult Index()
        {
            _logger.Debug("Someone trying to register");
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

            var succeeded = _candidateServiceProvider.Register(model);

            if (succeeded)
            {
                FormsAuthenticationTicket ticket = _ticketService.CreateTicket(model.EmailAddress, "Unactivated");

                _ticketService.AddTicket(Response.Cookies, ticket);

                return RedirectToAction("Activation");
            }

            return View(model);
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
                TempData["ReturnUrl"] = returnUrl;
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
                var ticket = _ticketService.CreateTicket(User.Identity.Name, "Activated");

                _ticketService.AddTicket(Response.Cookies, ticket);

                var returnUrl = TempData["ReturnUrl"] as string;

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Complete", "Register");
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
            return _candidateServiceProvider.IsUsernameAvailable(username.Trim()); //TODO Consider doing this everywhere
        }
    }
}
