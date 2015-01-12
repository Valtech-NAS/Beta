namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Applications;
    using Domain.Entities.Users;
    using Providers;
    using Validators;
    using ViewModels.Login;

    public class LoginMediator : MediatorBase, ILoginMediator
    {
        private readonly IUserDataProvider _userDataProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly LoginViewModelServerValidator _loginViewModelServerValidator;

        public LoginMediator(IUserDataProvider userDataProvider, ICandidateServiceProvider candidateServiceProvider, LoginViewModelServerValidator loginViewModelServerValidator)
        {
            _userDataProvider = userDataProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _loginViewModelServerValidator = loginViewModelServerValidator;
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
    }
}