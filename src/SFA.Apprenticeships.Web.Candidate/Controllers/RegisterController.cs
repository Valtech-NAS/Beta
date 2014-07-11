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
        private readonly IAddressSearchServiceProvider _addressSearchServiceProvider;
        private readonly ActivationViewModelServerValidator _activationViewModelServerValidator;
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(ISessionState session,
        RegisterViewModelServerValidator registerViewModelServerValidator,
            ActivationViewModelServerValidator activationViewModelServerValidator,
            IAddressSearchServiceProvider addressSearchServiceProvider,
            ICandidateServiceProvider candidateServiceProvider)
            : base(session)
        {
            _registerViewModelServerValidator = registerViewModelServerValidator;
            _activationViewModelServerValidator = activationViewModelServerValidator;
            _addressSearchServiceProvider = addressSearchServiceProvider;
            _candidateServiceProvider = candidateServiceProvider;
        }

        public ActionResult Index()
        {
        var validationResult = _registerViewModelServerValidator.Validate(registerView);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);
                return View(registerView);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Activation()
        {
            var model = new ActivationViewModel()
            {
                EmailAddress = "chris.monney@gmail.com"
            };

            return View(model);
        }

         [HttpPost]
        public ActionResult Activate(ActivationViewModel activationViewModel)
        {
            activationViewModel.IsActivated = _candidateServiceProvider.Activate(activationViewModel);

            var activatedResult = _activationViewModelServerValidator.Validate(activationViewModel);


            if (!activatedResult.IsValid)
            {
                ModelState.Clear();
                activatedResult.AddToModelState(ModelState, string.Empty);
                return View("Activation", activationViewModel);

            }

            return RedirectToAction("complete","register", activationViewModel.EmailAddress);
        }

        public ActionResult Complete(string email)
        {
            ViewBag.Message = email;

            return View(email);
        }
    }
}