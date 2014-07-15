using System.Web.Routing;
using SFA.Apprenticeships.Web.Candidate.ViewModels.Locations;

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
        private const string DummyEmailAddress = "chris.monney@gmail.com";

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
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel registerView)
        {
            var validationResult = _registerViewModelServerValidator.Validate(registerView);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);
                return View(registerView);
            }

            
            //todo remove this when address is fully integrated
            registerView.Address = new AddressViewModel()
            {
                GeoPoint = new GeoPointViewModel()
                {
                    Latitude = 51.7715110,
                    Longitude = -0.4534940
                }
            };

            var succeeded = _candidateServiceProvider.Register(registerView);

            return succeeded ? (ActionResult)RedirectToAction("Activation") : View(registerView);
        }

        [HttpGet]
        public ActionResult Activation()
        {
            var model = new ActivationViewModel()
            {
                EmailAddress = DummyEmailAddress
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Activate(ActivationViewModel activationViewModel)
        {
            activationViewModel.IsActivated = _candidateServiceProvider.Activate(activationViewModel);

            var activatedResult = _activationViewModelServerValidator.Validate(activationViewModel);

            if (activatedResult.IsValid) return RedirectToAction("Complete", "Register");

            ModelState.Clear();
            activatedResult.AddToModelState(ModelState, string.Empty);
            return View("Activation", activationViewModel);
        }

        public ActionResult Complete()
        {
            ViewBag.Message = DummyEmailAddress;

            return View();
        }
    }
}