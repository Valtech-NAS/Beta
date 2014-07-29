namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Bindings.Login
{
    using System;
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

        private const string EmailAddressTokenName = "EmailAddressToken";
        private const string EmailAddress = "login.feature@specbind.net";

        private const string PasswordTokenName = "PasswordToken";
        private const string Password = "?Password01!";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";

        private const string ActivationCodeTokenName = "ActivationCodeToken";
        private const string ActivationCode = "ACTIV1";

        private const string AccountUnlockCodeTokenName = "AccountUnlockCodeToken";
        private const string AccountUnlockCode = "UNLCK1";

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

        [Given("I registered an account and activated it and made two unsuccessful login attempts")]
        public void GivenIRegisteredAnAccountAndActivatedItAndMadeTwoUnsuccessfulLoginAttempts()
        {
            var candidate = new CandidateBuilder(Id, EmailAddress)
                .Build();

            var user = new UserBuilder(Id, EmailAddress, UserStatuses.Active)
                .WithLoginIncorrectAttempts(2)
                .Build();

            SetTokens(candidate, user);  
        }

        [Given("I locked my account")]
        public void GivenILockedMyAccount()
        {
            var candidate = new CandidateBuilder(Id, EmailAddress)
                .Build();

            var user = new UserBuilder(Id, EmailAddress, UserStatuses.Locked)
                .WithAccountUnlockCodeExpiry(DateTime.Now.AddDays(1))
                .WithAccountUnlockCode(AccountUnlockCode)
                .Build();

            SetTokens(candidate, user);
        }

        private void SetTokens(Candidate candidate, User user)
        {
            // Email.
            _tokenManager.SetToken(EmailAddressTokenName, candidate.RegistrationDetails.EmailAddress);

            // Password.
            _tokenManager.SetToken(PasswordTokenName, Password);
            _tokenManager.SetToken(InvalidPasswordTokenName, new string(Password.Reverse().ToArray()));

            // Activation, unlock codes etc.
            _tokenManager.SetToken(ActivationCodeTokenName, ActivationCode);
            _tokenManager.SetToken(AccountUnlockCodeTokenName, AccountUnlockCode);
        }
    }
}
