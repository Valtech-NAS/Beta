namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Login
{
    using System;
    using System.Linq;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using global::SpecBind.Helpers;
    using IoC;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class LoginCandidateDataBinding
    {
        private const string UserEmailAddress = "nas.exemplar+acceptancetests@gmail.com";
        private const string EmailAddressTokenName = "EmailAddressToken";
        private const string InvalidEmailTokenName = "InvalidEmailToken";
        private const string EmailTokenName = "EmailToken";
        private const string InvalidEmail = "invalid@gmail.com";
        private const string PasswordTokenName = "PasswordToken";
        private const string Password = "?Password01!";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";
        private const string ActivationCodeTokenName = "ActivationCodeToken";
        private const string ActivationCode = "ACTIV1";
        private const string AccountUnlockCodeTokenName = "AccountUnlockCodeToken";
        private const string AccountUnlockCode = "UNLCK1";
        private const string PasswordResetCodeTokenName = "PasswordResetCodeToken";
        private readonly ITokenManager _tokenManager;

        private readonly IUserReadRepository _userReadRepository;

        public LoginCandidateDataBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _userReadRepository = WebTestRegistry.Container.GetInstance<IUserReadRepository>();
        }

        //TODO: create a mechanism where we won't need to login - just get the webdriver and set the auth cookie directly or similar
        [Given("I registered an account and activated it")]
        public void GivenIRegisteredAnAccountAndActivatedIt()
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                .Build();

            var user = new UserBuilder(UserEmailAddress)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I registered an account but did not activate it")]
        public void GivenIRegisteredAnAccountButDidNotActivateIt()
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                .Build();

            var user = new UserBuilder(UserEmailAddress, UserStatuses.PendingActivation)
                .WithActivationCode(ActivationCode)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I made two unsuccessful login attempts")]
        public void GivenIMadeTwoUnsuccessfulLoginAttempts()
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                .Build();

            var user = new UserBuilder(UserEmailAddress)
                .WithLoginIncorrectAttempts(2)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I locked my account")]
        public void GivenILockedMyAccount()
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                .Build();

            var user = new UserBuilder(UserEmailAddress, UserStatuses.Locked)
                .WithAccountUnlockCodeExpiry(DateTime.Now.AddDays(1))
                .WithAccountUnlockCode(AccountUnlockCode)
                .Build();

            SetTokens(candidate, user);
        }


        [Given("I locked my account and my account unlock code has expired")]
        public void GivenILockedMyAccountAndMyAccountUnlockCodeHasExpired()
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                .Build();

            var user = new UserBuilder(UserEmailAddress, UserStatuses.Locked)
                .WithAccountUnlockCodeExpiry(DateTime.Now.AddDays(-7))
                .WithAccountUnlockCode(AccountUnlockCode)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I am signed out")]
        public void IAmSignedOut()
        {
            
        }

        [Then]
        public void ThenMyAccountUnlockCodeHasBeenRenewed()
        {
            var user = _userReadRepository.Get(UserEmailAddress);
            var accountUnlockCode = _tokenManager.GetTokenByKey(AccountUnlockCodeTokenName);

            // Ensure account unlock code has changed.
            accountUnlockCode.Should().NotBeNull();
            user.AccountUnlockCode.Should().NotBeNull();
            accountUnlockCode.Should().NotBe(user.AccountUnlockCode);

            // Ensure account unlock code has been renewed.
            user.AccountUnlockCodeExpiry.Should().BeAfter(DateTime.Now);
        }

        [Then(@"the user login incorrect attempts should be (.*)")]
        public void ThenTheUserLoginIncorrectAttemptsShouldBeX(int numLoginIncorrectAttempts)
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            user.LoginIncorrectAttempts.Should().Be(numLoginIncorrectAttempts);
        }

        [Then(@"the account unlock code and date should be set")]
        public void ThenTheAccountUnlockCodeAndDateShouldBeSet()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            user.AccountUnlockCode.Should().NotBeNullOrWhiteSpace();
            user.AccountUnlockCodeExpiry.Should().HaveValue();
        }

        [Then(@"the account unlock code and date should not be set")]
        public void ThenTheAccountUnlockCodeAndDateShouldNotBeSet()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            user.AccountUnlockCode.Should().BeNullOrWhiteSpace();
            user.AccountUnlockCodeExpiry.Should().NotHaveValue();
        }

        [Then(@"the password reset code and date should be set")]
        public void ThenThePasswordResetCodeAndDateShouldBeSet()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            user.PasswordResetCode.Should().NotBeNullOrWhiteSpace();
            user.PasswordResetCodeExpiry.Should().HaveValue();
        }

        [Then(@"the password reset code and date should not be set")]
        public void ThenThePasswordResetCodeAndDateShouldNotBeSet()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            user.PasswordResetCode.Should().BeNullOrWhiteSpace();
            user.PasswordResetCodeExpiry.Should().NotHaveValue();
        }

        [Then(@"I get the account unlock code")]
        public void ThenIGetTheAccountUnlockCode()
        {
            //_tokenManager.SetToken(AccountUnlockCodeTokenName, AccountUnlockCode);
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(AccountUnlockCodeTokenName, user.AccountUnlockCode);
            }
        }

        [Then(@"I get the same account unlock code")]
        public void ThenIGetTheSameAccountUnlockCode()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);
            var accountUnlockCode = _tokenManager.GetTokenByKey(AccountUnlockCodeTokenName);

            // Ensure account unlock code has changed.
            accountUnlockCode.Should().NotBeNull();
            user.AccountUnlockCode.Should().NotBeNull();
            accountUnlockCode.Should().Be(user.AccountUnlockCode);
        }

        [Then(@"I get the password reset code")]
        public void ThenIGetThePasswordResetCode()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(PasswordResetCodeTokenName, user.PasswordResetCode);
            }
        }

        #region Helpers

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
            _tokenManager.SetToken(InvalidEmailTokenName, InvalidEmail);
        }

        #endregion
    }
}