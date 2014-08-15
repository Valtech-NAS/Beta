namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common.Controllers;
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

    public class LoginController : SfaControllerBase
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
            Session.Clear();

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                UserServiceProvider.SetReturnUrlCookie(HttpContext, returnUrl);
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

            // Authenticate candidate.
            var candidate = _candidateServiceProvider.Authenticate(model);
            var userStatus = _candidateServiceProvider.GetUserStatus(model.EmailAddress);

            if (candidate != null)
            {
                return RedirectOnAuthenticated(candidate, userStatus);
            }

            if (userStatus == UserStatuses.Locked)
            {
                return RedirectOnAccountLocked(model.EmailAddress);
            }

            ModelState.Clear();
            ModelState.AddModelError("EmailAddress", LoginPageMessages.AuthenticationFailedErrorText);

            return View(model);
        }

        [HttpGet]
        public ActionResult Unlock()
        {
            var emailAddress = PopContextData(ContextDataItemNames.EmailAddress);

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

            PushContextData(ContextDataItemNames.EmailAddress, model.EmailAddress);

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
            if (userStatus == UserStatuses.PendingActivation)
            {
                return RedirectOnPendingActivation();
            }
          
            // Redirect to return URL (if any).
            var returnUrl = UserServiceProvider.GetReturnUrl(HttpContext);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToReturnUrl(returnUrl);
            }

            // Redirect to last viewed vacancy (if any).
            if (_candidateServiceProvider.LastViewedVacancyId.HasValue)
            {
                return RedirectToLastViewedVacancy(
                    candidate, _candidateServiceProvider.LastViewedVacancyId.Value);
            }

            return RedirectToRoute(RouteNames.MyApplications);
        }

        private ActionResult RedirectToLastViewedVacancy(Candidate candidate, int lastViewedVacancyId)
        {
            // Always clear last viewed vacancy.
            _candidateServiceProvider.LastViewedVacancyId = null;

            var applicationStatus = _candidateServiceProvider.GetApplicationStatus(candidate.EntityId,
                lastViewedVacancyId);

            if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
            {
                return RedirectToAction(
                    "Apply", "Application", new
                    {
                        id = lastViewedVacancyId
                    });
            }

            return RedirectToAction(
                "Details", "VacancySearch", new
                {
                    id = lastViewedVacancyId
                });
        }

        private ActionResult RedirectToReturnUrl(string returnUrl)
        {
            UserServiceProvider.DeleteReturnUrlCookie(HttpContext);

            return Redirect(returnUrl);
        }

        private ActionResult RedirectOnPendingActivation()
        {
            return RedirectToAction("Activation", "Register");
        }

        private ActionResult RedirectOnAccountLocked(string emailAddress)
        {
            PushContextData(ContextDataItemNames.EmailAddress, emailAddress);

            return RedirectToAction("Unlock");
        }

        #endregion
    }
}
