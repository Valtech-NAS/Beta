namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Login;

    public class LoginViewModelBuilder
    {
        public const string ValidEmailAddress = "user@users.com";
        public const string ValidPassword = "Password01";
        public const string InvalidEmailAddress = "invalid@users.com";
        public const string InvalidPassword = "NotPassword01";

        private string _emailAddress;
        private string _password;

        public LoginViewModelBuilder WithValidCredentials()
        {
            _emailAddress = ValidEmailAddress;
            _password = ValidPassword;
            return this;
        }

        public LoginViewModelBuilder WithInvalidCredentials()
        {
            _emailAddress = InvalidEmailAddress;
            _password = InvalidPassword;
            return this;
        }

        public LoginViewModelBuilder WithEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
            return this;
        }

        public LoginViewModelBuilder WithPassword(string password)
        {
            _password = password;
            return this;
        }
        
        public LoginViewModel Build()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = _emailAddress,
                Password = _password
            };

            return viewModel;
        }
    }
}