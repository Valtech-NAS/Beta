namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using FluentValidation.Mvc;
    using Infrastructure.Azure.Session;
    using Validators;
    using ViewModels.Register;
    using Providers;
    public class RegisterController : SfaControllerBase
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(ISessionState session,
                                    RegisterViewModelServerValidator registerViewModelServerValidator,
                                    ActivationViewModelServerValidator activationViewModelServerValidator,
                                    ICandidateServiceProvider candidateServiceProvider)
            : base(session)
        {
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel registerView)
        {
            registerView.IsUsernameAvailable = IsUsernameAvailable(registerView.EmailAddress);

            var validationResult = _registerViewModelServerValidator.Validate(registerView);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);
                return View(registerView);
            }

            var succeeded = _candidateServiceProvider.Register(registerView);

            TempData["EmailAddress"] = registerView.EmailAddress;

            //FormsAuthentication.SetAuthCookie(registerView.EmailAddress, false);

            return succeeded ? (ActionResult)RedirectToAction("Activation") : View(registerView);
        }

        [HttpGet]
        public ActionResult Activation()
        {
            var model = new ActivationViewModel
            {
                EmailAddress = TempData["EmailAddress"].ToString()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Activate(ActivationViewModel activationViewModel)
        {
            activationViewModel.IsActivated = _candidateServiceProvider.Activate(activationViewModel);

            var activatedResult = _activationViewModelServerValidator.Validate(activationViewModel);

            if (activatedResult.IsValid)
            {
                TempData["EmailAddress"] = activationViewModel.EmailAddress;
                return RedirectToAction("Complete", "Register");
            }

            ModelState.Clear();
            activatedResult.AddToModelState(ModelState, string.Empty);
            return View("Activation", activationViewModel);
        }

        public ActionResult Complete()
        {
            ViewBag.Message = TempData["EmailAddress"].ToString();

            return View();
        }

        public JsonResult CheckUsername(CheckUsernameViewModel model)
        {
            if (!ModelState.IsValid) return Json(new {Result = false}, JsonRequestBehavior.AllowGet);
            var usernameIsAvailable = IsUsernameAvailable(model.Email);
            return Json(new { Result = usernameIsAvailable }, JsonRequestBehavior.AllowGet);
        }

        private bool IsUsernameAvailable(string username)
        {
            return _candidateServiceProvider.IsUsernameAvailable(username.Trim()); //TODO Consider doing this everywhere
        }
    }
}