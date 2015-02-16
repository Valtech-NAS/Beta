namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Login;
    using Domain.Entities.Users;

    public class LoginResultViewModelBuilder
    {
        private readonly UserStatuses _userStatus;
        private readonly bool _isAuthenticated;

        private string _emailAddress;
        private string _viewModelMessage;
        private string _acceptedTermsAndConditionsVersion;
        private bool _mobileVerificationRequired;

        public LoginResultViewModelBuilder(UserStatuses userStatus = UserStatuses.Active, bool isAuthenticated = true)
        {
            _userStatus = userStatus;
            _isAuthenticated = isAuthenticated;
        }

        public LoginResultViewModelBuilder WithEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
            return this;
        }

        public LoginResultViewModelBuilder WithViewModelMessage(string viewModelMessage)
        {
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public LoginResultViewModelBuilder WithAcceptedTermsAndConditionsVersion(string acceptedTermsAndConditionsVersion)
        {
            _acceptedTermsAndConditionsVersion = acceptedTermsAndConditionsVersion;
            return this;
        }

        public LoginResultViewModelBuilder MobileVerificationRequired()
        {
            _mobileVerificationRequired = true;
            return this;
        }

        public LoginResultViewModel Build()
        {
            var viewModel = new LoginResultViewModel
            {
                UserStatus = _userStatus,
                IsAuthenticated = _isAuthenticated,
                EmailAddress = _emailAddress,
                ViewModelMessage = _viewModelMessage,
                AcceptedTermsAndConditionsVersion = _acceptedTermsAndConditionsVersion,
                MobileVerificationRequired = _mobileVerificationRequired
            };

            return viewModel;
        }
    }
}