namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentValidation.Mvc;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginController : CandidateControllerBase
    {
        private readonly AccountUnlockViewModelServerValidator _accountUnlockViewModelServerValidator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginController(
            ISessionStateProvider session,
            IUserServiceProvider userServiceProvider,
            LoginViewModelServerValidator loginViewModelServerValidator,
            AccountUnlockViewModelServerValidator accountUnlockViewModelServerValidator,
            ICandidateServiceProvider candidateServiceProvider)
            : base(session, userServiceProvider)
        {
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _accountUnlockViewModelServerValidator = accountUnlockViewModelServerValidator;
            _candidateServiceProvider = candidateServiceProvider;
        }

        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            UserServiceProvider.DeleteAllCookies(HttpContext);

            ClearSession();

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserServiceProvider.SetCookie(HttpContext, ContextDataItemNames.ReturnUrl, returnUrl);
            }        

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            // todo: refactor - too much going on here ILoginServiceProvider
            // Validate view model.
            var validationResult = _loginViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            // Authenticate candidate.
            var candidate = _candidateServiceProvider.Authenticate(model);
            var userStatus = _candidateServiceProvider.GetUserStatus(model.EmailAddress);

            if (candidate != null)
            {
                return RedirectOnAuthenticated(candidate, userStatus);
            }

            if (userStatus == UserStatuses.Locked)
            {
                PushContextData(TempDataItemNames.EmailAddress, model.EmailAddress);

                return RedirectToAction("Unlock");
            }

            ModelState.Clear();
            ModelState.AddModelError("EmailAddress", LoginPageMessages.AuthenticationFailedErrorText);

            return View(model);
        }

        [HttpGet]
        public ActionResult Unlock()
        {
            var emailAddress = PopContextData<string>(TempDataItemNames.EmailAddress);

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return RedirectToAction("Index");
            }

            return View(new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            });
        }

        [HttpPost]
        public ActionResult Unlock(AccountUnlockViewModel model)
        {
            // todo: refactor - too much going on here ILoginServiceProvider
            var validationResult = _accountUnlockViewModelServerValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                ModelState.Clear();
                validationResult.AddToModelState(ModelState, string.Empty);

                return View(model);
            }

            var verified = _candidateServiceProvider.VerifyAccountUnlockCode(model);

            if (verified)
            {
                SetUserMessage(AccountUnlockPageMessages.AccountUnlockedText);

                return RedirectToAction("Index");
            }

            ModelState.Clear();
            ModelState.AddModelError("AccountUnlockCode", AccountUnlockPageMessages.WrongAccountUnlockCodeErrorText);

            return View(model);
        }

        public ActionResult ResendAccountUnlockCode(string emailAddress)
        {
            var model = new AccountUnlockViewModel
            {
                EmailAddress = emailAddress
            };

            _candidateServiceProvider.RequestAccountUnlockCode(model);

            PushContextData(TempDataItemNames.EmailAddress, model.EmailAddress);

            SetUserMessage(string.Format(AccountUnlockPageMessages.AccountUnlockCodeResent, emailAddress));

            return RedirectToAction("Unlock");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            SetUserMessage(SignOutPageMessages.SignOutMessageText);

            return RedirectToAction("Index");
        }

        #region Helpers

        private ActionResult RedirectOnAuthenticated(Candidate candidate, UserStatuses userStatus)
        {
            // todo: refactor - too much going on here ILoginServiceProvider
            if (userStatus == UserStatuses.PendingActivation)
            {
                return RedirectToAction("Activation", "Register");
            }
          
            // Redirect to return URL (if any).
            var returnUrl = UserServiceProvider.GetCookie(HttpContext, ContextDataItemNames.ReturnUrl);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserServiceProvider.DeleteCookie(HttpContext, ContextDataItemNames.ReturnUrl);

                return Redirect(returnUrl);
            }

            // Redirect to last viewed vacancy (if any).
            var lastViewedVacancyId = PopContextData<int?>(ContextDataItemNames.LastViewedVacancyId);

            if (lastViewedVacancyId.HasValue)
            {
                var applicationStatus = _candidateServiceProvider.GetApplicationStatus(candidate.EntityId, lastViewedVacancyId.Value);

                if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
                {
                    return RedirectToAction("Apply", "Application", new { id = lastViewedVacancyId });
                }

                return RedirectToAction("Details", "VacancySearch", new { id = lastViewedVacancyId });
            }

            return RedirectToRoute(RouteNames.MyApplications);
        }

        #endregion
    }
}
