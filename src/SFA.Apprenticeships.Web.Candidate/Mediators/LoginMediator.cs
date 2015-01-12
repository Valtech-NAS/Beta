namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System.Web;
    using Common.Constants;
    using Common.Providers;
    using Common.Services;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginMediator : MediatorBase, ILoginMediator
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;
        private readonly AccountUnlockViewModelServerValidator _accountUnlockViewModelServerValidator;
        private readonly ResendAccountUnlockCodeViewModelServerValidator _resendAccountUnlockCodeViewModelServerValidator;


        public LoginMediator(IUserDataProvider userDataProvider, ICandidateServiceProvider candidateServiceProvider,
            IAuthenticationTicketService authenticationTicketService,
            LoginViewModelServerValidator loginViewModelServerValidator, 
            AccountUnlockViewModelServerValidator accountUnlockViewModelServerValidator,
            ResendAccountUnlockCodeViewModelServerValidator resendAccountUnlockCodeViewModelServerValidator)
        {
            _userDataProvider = userDataProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _authenticationTicketService = authenticationTicketService;
            _loginViewModelServerValidator = loginViewModelServerValidator;
            _accountUnlockViewModelServerValidator = accountUnlockViewModelServerValidator;
            _resendAccountUnlockCodeViewModelServerValidator = resendAccountUnlockCodeViewModelServerValidator;
        }

        public MediatorResponse Index(LoginViewModel viewModel)
        {
            var validationResult = _loginViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(Codes.Login.Index.ValidationError, validationResult: validationResult);
            }

            var result = _candidateServiceProvider.Login(viewModel);

            if (result.UserStatus == UserStatuses.Locked)
            {
                _userDataProvider.Push(UserDataItemNames.UnlockEmailAddress, result.EmailAddress);

                return GetMediatorResponse(Codes.Login.Index.AccountLocked);
            }

            if (result.IsAuthenticated)
            {
                _userDataProvider.SetUserContext(result.EmailAddress, result.FullName);

                if (result.UserStatus == UserStatuses.PendingActivation)
                {
                    return GetMediatorResponse(Codes.Login.Index.PendingActivation);
                }

                // Redirect to session return URL (if any).
                var sessionReturnUrl = _userDataProvider.Pop(UserDataItemNames.SessionReturnUrl);

                if (!string.IsNullOrWhiteSpace(sessionReturnUrl))
                {
                    return GetMediatorResponse(Codes.Login.Index.ReturnUrl, parameters: sessionReturnUrl);
                }

                // Redirect to return URL (if any).
                var returnUrl = _userDataProvider.Pop(UserDataItemNames.ReturnUrl);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return GetMediatorResponse(Codes.Login.Index.ReturnUrl, parameters: returnUrl);
                }

                // Redirect to last viewed vacancy (if any).
                var lastViewedVacancyId = _userDataProvider.Pop(UserDataItemNames.LastViewedVacancyId);

                if (lastViewedVacancyId != null)
                {
                    var candidate = _candidateServiceProvider.GetCandidate(result.EmailAddress);

                    var applicationStatus = _candidateServiceProvider.GetApplicationStatus(candidate.EntityId, int.Parse(lastViewedVacancyId));

                    if (applicationStatus.HasValue && applicationStatus.Value == ApplicationStatuses.Draft)
                    {
                        return GetMediatorResponse(Codes.Login.Index.ApprenticeshipApply, parameters: lastViewedVacancyId);
                    }

                    return GetMediatorResponse(Codes.Login.Index.ApprenticeshipDetails, parameters: lastViewedVacancyId);
                }

                return GetMediatorResponse(Codes.Login.Index.Ok);
            }

            return GetMediatorResponse(Codes.Login.Index.LoginFailed, parameters: result.ViewModelMessage);
        }


        public MediatorResponse<AccountUnlockViewModel> Unlock(AccountUnlockViewModel accountUnlockView)
        {
            var validationResult = _accountUnlockViewModelServerValidator.Validate(accountUnlockView);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(Codes.Login.Unlock.ValidationError, accountUnlockView, validationResult);
            }

            var accountUnlockViewModel = _candidateServiceProvider.VerifyAccountUnlockCode(accountUnlockView);
            switch (accountUnlockViewModel.Status)
            {
                case AccountUnlockState.Ok:
                    return GetMediatorResponse(Codes.Login.Unlock.UnlockedSuccessfully, accountUnlockView);
                case AccountUnlockState.UserInIncorrectState:
                    return GetMediatorResponse(Codes.Login.Unlock.UserInIncorrectState, accountUnlockView);
                case AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid:
                    return GetMediatorResponse(Codes.Login.Unlock.AccountEmailAddressOrUnlockCodeInvalid, accountUnlockView, AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error);
                case AccountUnlockState.AccountUnlockCodeExpired:
                    return GetMediatorResponse(Codes.Login.Unlock.AccountUnlockCodeExpired, accountUnlockView, AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning);
                default:
                    return GetMediatorResponse(Codes.Login.Unlock.AccountUnlockFailed, accountUnlockView, AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning);
            }
        }

        public MediatorResponse<AccountUnlockViewModel> Resend(AccountUnlockViewModel accountUnlockViewModel)
        {
            var validationResult = _resendAccountUnlockCodeViewModelServerValidator.Validate(accountUnlockViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(Codes.Login.Resend.ValidationError, accountUnlockViewModel);
            }

            accountUnlockViewModel = _candidateServiceProvider.RequestAccountUnlockCode(accountUnlockViewModel);
            _userDataProvider.Push(UserDataItemNames.EmailAddress, accountUnlockViewModel.EmailAddress);

            if (accountUnlockViewModel.HasError())
            {
                return GetMediatorResponse(Codes.Login.Resend.ResendFailed, accountUnlockViewModel, AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(Codes.Login.Resend.ResentSuccessfully, accountUnlockViewModel, string.Format(AccountUnlockPageMessages.AccountUnlockCodeResent, accountUnlockViewModel.EmailAddress), UserMessageLevel.Success);
        }
    }
}