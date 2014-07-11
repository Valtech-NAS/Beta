namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using FluentValidation.Mvc;
    using Infrastructure.Azure.Session;
    using Validators;
    using ViewModels.Register;

    public class RegisterController : SfaControllerBase
    {
        private readonly RegisterViewModelServerValidator _registerViewModelServerValidator;

        public RegisterController(ISessionState sessionState, RegisterViewModelServerValidator registerViewModelServerValidator) : base(sessionState)
        {
            _registerViewModelServerValidator = registerViewModelServerValidator;
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

            return View();
        }
    }
}