namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Account;

    public class AccountController : CandidateControllerBase
    {
        private readonly IAccountProvider _accountProvider;
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator;

        public AccountController(
            IAccountProvider accountProvider,
            SettingsViewModelServerValidator settingsViewModelServerValidator)
        {
            _accountProvider = accountProvider;
            _settingsViewModelServerValidator = settingsViewModelServerValidator;
        }

        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public ActionResult Index()
        {
            var model = _accountProvider.GetSettingsViewModel(UserContext.CandidateId);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SettingsViewModel model)
        {
            var validationResult = _settingsViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            var saved = _accountProvider.SaveSettings(UserContext.CandidateId, model);

            if (!saved)
            {
                ModelState.Clear();
                ModelState.AddModelError("EmailAddress", AccountPageMessages.UpdateFailed);

                return View(model);
            }

            UserData.SetUserContext(UserContext.UserName, model.Firstname + " " + model.Lastname);

            return RedirectToAction("Index");
        }
    }
}
