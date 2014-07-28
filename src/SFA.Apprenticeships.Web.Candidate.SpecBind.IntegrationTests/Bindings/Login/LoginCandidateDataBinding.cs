namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Bindings.Login
{
    using System.Linq;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using global::SpecBind.Helpers;
    using TechTalk.SpecFlow;

    [Binding]
    public class LoginCandidateDataBinding
    {
        private const string Id = "00000000-0000-0000-0000-000000000001"; // CRITICAL: must match an AD user name.
        private const string EmailAddress = "login.feature@specbind.net";
        private const string Password = "?Password01!";
        private const string ActivationCode = "123ABC";

        private const string EmailTokenName = "EmailAddressToken";
        private const string PasswordTokenName = "PasswordToken";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";
        private const string ActivationCodeTokenName = "ActivationCodeToken";
        
        private readonly ITokenManager _tokenManager;

        public LoginCandidateDataBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [Given("I registered an account but did not activate it")]
        public void GivenIRegisteredAnAccountButDidNotActivateIt()
        {
            var candidate = new CandidateBuilder(Id, EmailAddress)
                .Build();

            var user = new UserBuilder(Id, EmailAddress, UserStatuses.PendingActivation)
                .WithActivationCode(ActivationCode)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I registered an account and activated it")]
        public void GivenIRegisteredAnAccountAndActivatedIt()
        {
            var candidate = new CandidateBuilder(Id, EmailAddress)
                .Build();

            var user = new UserBuilder(Id, EmailAddress, UserStatuses.Active)
                .Build();

            SetTokens(candidate, user);
        }

        private void SetTokens(Candidate candidate, User user)
        {
            _tokenManager.SetToken(EmailTokenName, candidate.RegistrationDetails.EmailAddress);
            _tokenManager.SetToken(PasswordTokenName, Password);
            _tokenManager.SetToken(InvalidPasswordTokenName, new string(Password.Reverse().ToArray()));
            _tokenManager.SetToken(ActivationCodeTokenName, ActivationCode);
        }
    }
}
