namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Account;
    using Mediators.Login;
    using ViewModels.Account;
    using ViewModels.Login;

    public class AccountController : CandidateControllerBase
    {
        private readonly IAccountMediator _accountMediator;
        private readonly IDismissPlannedOutageMessageCookieProvider _dismissPlannedOutageMessageCookieProvider;

        public AccountController(IAccountMediator accountMediator, IDismissPlannedOutageMessageCookieProvider dismissPlannedOutageMessageCookieProvider)
        {
            _accountMediator = accountMediator;
            _dismissPlannedOutageMessageCookieProvider = dismissPlannedOutageMessageCookieProvider;
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Index(UserContext.CandidateId, UserData.Pop(CandidateDataItemNames.DeletedVacancyId), UserData.Pop(CandidateDataItemNames.DeletedVacancyTitle));
                return View(response.ViewModel);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Settings()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Settings(UserContext.CandidateId);
                return View(response.ViewModel);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Settings(SettingsViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.SaveSettings(UserContext.CandidateId, model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case AccountMediatorCodes.Settings.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.Settings.SaveError:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.Settings.MobileVerificationRequired:
                        return RedirectToAction("VerifyMobile");
                    case AccountMediatorCodes.Settings.Success:
                        UserData.SetUserContext(UserContext.UserName, response.ViewModel.Firstname + " " + response.ViewModel.Lastname, UserContext.AcceptedTermsAndConditionsVersion);
                        SetUserMessage(AccountPageMessages.SettingsUpdated);
                        return RedirectToRoute(CandidateRouteNames.Settings);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        //todo: 1.6: mobile verification
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> VerifyMobile()
        {
            var response = _accountMediator.VerifyMobile(UserContext.CandidateId);
            return await Task.Run<ActionResult>(() => View(response.ViewModel));
        }

       // todo: 1.6: mobile verification
        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [AllowReturnUrl(Allow = false)]
        [MultipleFormActionsButton(Name = "VerifyMobileAction", Argument = "VerifyMobile")]
        [ApplyWebTrends]
        public async Task<ActionResult> VerifyMobile(VerifyMobileViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.VerifyMobile(UserContext.CandidateId, model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case AccountMediatorCodes.VerifyMobile.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.InvalidCode:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationCodeInvalid, UserMessageLevel.Error);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.Error:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationError, UserMessageLevel.Error);
                        return View(response.ViewModel);
                    case AccountMediatorCodes.VerifyMobile.Success:
                        SetUserMessage(VerifyMobilePageMessages.MobileVerificationSuccessText);
                        //todo: return url should work 
                        return Redirect(HttpUtility.UrlDecode(response.Parameters.ToString()));
                        //return RedirectToRoute(CandidateRouteNames.Settings); //todo: return url 
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AllowReturnUrl(Allow = false)]
        [MultipleFormActionsButton(Name = "VerifyMobileAction", Argument = "Resend")]
        [ApplyWebTrends]
        public async Task<ActionResult> Resend(VerifyMobileViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Resend( UserContext.CandidateId, model);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Resend.Error:
                    case AccountMediatorCodes.Resend.ResendNotRequired:
                    case AccountMediatorCodes.Resend.ResentSuccessfully:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
                return RedirectToAction("VerifyMobile"); 
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Archive(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Archive(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Archive.SuccessfullyArchived:
                        SetUserMessage(MyApplicationsPageMessages.ApplicationArchived);
                        break;
                    case AccountMediatorCodes.Archive.ErrorArchiving:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Delete(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Delete(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Delete.SuccessfullyDeleted:
                        UserData.Push(CandidateDataItemNames.DeletedVacancyId, id.ToString(CultureInfo.InvariantCulture));
                        UserData.Push(CandidateDataItemNames.DeletedVacancyTitle, response.Message.Text);
                        break;
                    case AccountMediatorCodes.Delete.AlreadyDeleted:
                    case AccountMediatorCodes.Delete.ErrorDeleting:
                    case AccountMediatorCodes.Delete.SuccessfullyDeletedExpiredOrWithdrawn:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> DismissPlannedOutageMessage(bool isJavascript)
        {
            return await Task.Run<ActionResult>(() =>
            {
                _dismissPlannedOutageMessageCookieProvider.SetCookie(HttpContext);

                if (isJavascript)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                if (Request.UrlReferrer != null)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }

                return RedirectToRoute(RouteNames.SignIn);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> DismissTraineeshipPrompts()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.DismissTraineeshipPrompts(UserContext.CandidateId);

                switch (response.Code)
                {
                    case AccountMediatorCodes.DismissTraineeshipPrompts.SuccessfullyDismissed:
                        break;
                    case AccountMediatorCodes.DismissTraineeshipPrompts.ErrorDismissing:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }


        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> Track(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.Track(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.Track.SuccessfullyTracked:
                    case AccountMediatorCodes.Track.ErrorTracking:
                        // Tracking an application is 'best efforts'. Errors are not reported to the user.
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> UpdatedTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {                
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    var routeValueDictionary = new {ReturnUrl = returnUrl};
                    return View("Terms", routeValueDictionary);
                }

                return View("Terms");
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> AcceptTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (UserContext == null)
                {
                    //Check needed as AuthorizeCandidate attribute not on action
                    return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
                }

                var response = _accountMediator.AcceptTermsAndConditions(UserContext.CandidateId);

                switch (response.Code)
                {
                    case AccountMediatorCodes.AcceptTermsAndConditions.SuccessfullyAccepted:
                    case AccountMediatorCodes.AcceptTermsAndConditions.AlreadyAccepted:
                        break;
                    case AccountMediatorCodes.AcceptTermsAndConditions.ErrorAccepting:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]

        public async Task<ActionResult> DeclineTermsAndConditions(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                SetUserMessage(SignOutPageMessages.MustAcceptUpdatedTermsAndConditions, UserMessageLevel.Warning);

                return !string.IsNullOrEmpty(returnUrl)
                    ? RedirectToRoute(RouteNames.SignOut, new {ReturnUrl = returnUrl})
                    : RedirectToRoute(RouteNames.SignOut);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> ApprenticeshipVacancyDetails(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.ApprenticeshipVacancyDetails(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.VacancyDetails.Available:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });

                    case AccountMediatorCodes.VacancyDetails.Unavailable:
                    case AccountMediatorCodes.VacancyDetails.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }

        [OutputCache(CacheProfile = CacheProfiles.None)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [ApplyWebTrends]
        public async Task<ActionResult> TraineeshipVacancyDetails(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accountMediator.TraineeshipVacancyDetails(UserContext.CandidateId, id);

                switch (response.Code)
                {
                    case AccountMediatorCodes.VacancyDetails.Available:
                        return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id });

                    case AccountMediatorCodes.VacancyDetails.Unavailable:
                    case AccountMediatorCodes.VacancyDetails.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;

                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }
    }
}
