namespace SFA.Apprenticeships.Web.Candidate.Mediators.Login
{
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginMediator : MediatorBase, ILoginMediator
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IUserDataProvider _userDataProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;
        private readonly AccountUnlockViewModelServerValidator _accountUnlockViewModelServerValidator;
        private readonly ResendAccountUnlockCodeViewModelServerValidator _resendAccountUnlockCodeViewModelServerValidator;

        public LoginMediator(IUserDataProvider userDataProvider, 
            ICandidateServiceProvider candidateServiceProvider,
            IConfigurationManager configurationManager,
            LoginViewModelServerValidator loginViewModelServerValidator, 
            AccountUnlockViewModelServerValidator accountUnlockViewModelServerValidator,
            ResendAccountUnlockCodeViewModelServerValidator resendAccountUnlockCodeViewModelServerValidator)
        {
            _userDataProvider = userDataProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _configurationManager = configurationManager;
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _accountUnlockViewModelServerValidator = accountUnlockViewModelServerValidator;
            _resendAccountUnlockCodeViewModelServerValidator = resendAccountUnlockCodeViewModelServerValidator;
        }

        public MediatorResponse<LoginResultViewModel> Index(LoginViewModel viewModel)
        {
            var validationResult = _loginViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse<LoginResultViewModel>(LoginMediatorCodes.Index.ValidationError, null, validationResult);
            }

            var result = _candidateServiceProvider.Login(viewModel);

            if (result.UserStatus.HasValue)
            {
                if (result.UserStatus == UserStatuses.Locked)
                {
                    _userDataProvider.Push(UserDataItemNames.UnlockEmailAddress, result.EmailAddress);

                    return GetMediatorResponse(LoginMediatorCodes.Index.AccountLocked, result);
                }

                if (result.IsAuthenticated)
                {
                    _userDataProvider.SetUserContext(result.EmailAddress, result.FullName, result.AcceptedTermsAndConditionsVersion);

                    if (result.UserStatus == UserStatuses.PendingActivation)
                    {
                        return GetMediatorResponse(LoginMediatorCodes.Index.PendingActivation, result);
                    }

                    // Redirect to session return URL (if any).
                    var returnUrl = _userDataProvider.Pop(UserDataItemNames.SessionReturnUrl) ?? _userDataProvider.Pop(UserDataItemNames.ReturnUrl);

                    if (result.AcceptedTermsAndConditionsVersion != _configurationManager.GetAppSetting<string>(Settings.TermsAndConditionsVersion))
                    {
                        return !string.IsNullOrEmpty(returnUrl)
                            ? GetMediatorResponse(LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted, result, parameters: returnUrl)
                            : GetMediatorResponse(LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted, result);
                    }

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return GetMediatorResponse(LoginMediatorCodes.Index.ReturnUrl, result, parameters: returnUrl);
                    }

                    // Redirect to last viewed vacancy (if any).
                    var lastViewedVacancyId = _userDataProvider.Pop(CandidateDataItemNames.LastViewedVacancyId);

                    if (lastViewedVacancyId != null)
                    {
                        var candidate = _candidateServiceProvider.GetCandidate(result.EmailAddress);

                        var applicationStatus = _candidateServiceProvider.GetApplicationStatus(candidate.EntityId, int.Parse(lastViewedVacancyId));

                        if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
                        {
                            return GetMediatorResponse(LoginMediatorCodes.Index.ApprenticeshipApply, result, parameters: lastViewedVacancyId);
                        }

                        return GetMediatorResponse(LoginMediatorCodes.Index.ApprenticeshipDetails, result, parameters: lastViewedVacancyId);
                    }

                    return GetMediatorResponse(LoginMediatorCodes.Index.Ok, result);
                }                
            }

            return GetMediatorResponse(LoginMediatorCodes.Index.LoginFailed, result, parameters: result.ViewModelMessage);
        }


        public MediatorResponse<AccountUnlockViewModel> Unlock(AccountUnlockViewModel accountUnlockView)
        {
            var validationResult = _accountUnlockViewModelServerValidator.Validate(accountUnlockView);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.Unlock.ValidationError, accountUnlockView, validationResult);
            }

            var accountUnlockViewModel = _candidateServiceProvider.VerifyAccountUnlockCode(accountUnlockView);
            switch (accountUnlockViewModel.Status)
            {
                case AccountUnlockState.Ok:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.UnlockedSuccessfully, accountUnlockView);
                case AccountUnlockState.UserInIncorrectState:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.UserInIncorrectState, accountUnlockView);
                case AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.AccountEmailAddressOrUnlockCodeInvalid, accountUnlockView, AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error);
                case AccountUnlockState.AccountUnlockCodeExpired:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.AccountUnlockCodeExpired, accountUnlockView, AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                default:
                    return GetMediatorResponse(LoginMediatorCodes.Unlock.AccountUnlockFailed, accountUnlockView, AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
            }
        }

        public MediatorResponse<AccountUnlockViewModel> Resend(AccountUnlockViewModel accountUnlockViewModel)
        {
            var validationResult = _resendAccountUnlockCodeViewModelServerValidator.Validate(accountUnlockViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(LoginMediatorCodes.Resend.ValidationError, accountUnlockViewModel, validationResult);
            }

            accountUnlockViewModel = _candidateServiceProvider.RequestAccountUnlockCode(accountUnlockViewModel);
            _userDataProvider.Push(UserDataItemNames.EmailAddress, accountUnlockViewModel.EmailAddress);

            if (accountUnlockViewModel.HasError())
            {
                if (accountUnlockViewModel.Status == AccountUnlockState.UserInIncorrectState || accountUnlockViewModel.Status == AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid)
                {
                    return GetMediatorResponse(LoginMediatorCodes.Resend.ResentSuccessfully, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success);
                }
                return GetMediatorResponse(LoginMediatorCodes.Resend.ResendFailed, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(LoginMediatorCodes.Resend.ResentSuccessfully, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success);
        }
    }
}