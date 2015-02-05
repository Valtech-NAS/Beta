namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Login
{
    using System;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using global::SpecBind.Helpers;
    using IoC;
    using TechTalk.SpecFlow;

    [Binding]
    public class LoginCandidateDataBinding
    {
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
            var candidate = new CandidateBuilder(BindingData.UserEmailAddress)
                .Build();

            var user = new UserBuilder(BindingData.UserEmailAddress)
                .Build();
            
            SetTokens(candidate);
        }

        [Given("I registered an account but did not activate it")]
        public void GivenIRegisteredAnAccountButDidNotActivateIt()
        {
            var candidate = new CandidateBuilder(BindingData.UserEmailAddress)
                .Build();

            var user = new UserBuilder(BindingData.UserEmailAddress, UserStatuses.PendingActivation)
                .WithActivationCode(BindingData.ActivationCode).Build();
            
            SetTokens(candidate);
        }

        [Given("I made two unsuccessful login attempts")]
        public void GivenIMadeTwoUnsuccessfulLoginAttempts()
        {
            var candidate = new CandidateBuilder(BindingData.UserEmailAddress)
                .Build();

            var user = new UserBuilder(BindingData.UserEmailAddress)
                .WithLoginIncorrectAttempts(2).Build();
            
            SetTokens(candidate);
        }

        [Given("I locked my account")]
        public void GivenILockedMyAccount()
        {
            var candidate = new CandidateBuilder(BindingData.UserEmailAddress)
                .Build();

            var user = new UserBuilder(BindingData.UserEmailAddress, UserStatuses.Locked)
                .WithAccountUnlockCodeExpiry(DateTime.Now.AddDays(1))
                .WithAccountUnlockCode(BindingData.AccountUnlockCode)
                .Build();
            
            SetTokens(candidate);
        }


        [Given("I locked my account and my account unlock code has expired")]
        public void GivenILockedMyAccountAndMyAccountUnlockCodeHasExpired()
        {
            var candidate = new CandidateBuilder(BindingData.UserEmailAddress)
                .Build();

            var user = new UserBuilder(BindingData.UserEmailAddress, UserStatuses.Locked)
                .WithAccountUnlockCodeExpiry(DateTime.Now.AddDays(-7))
                .WithAccountUnlockCode(BindingData.AccountUnlockCode).Build();

            SetTokens(candidate);
        }

        //TODO: Dead code?
        [Given("I am signed out")]
        public void IAmSignedOut()
        {
            
        }

        [Then]
        public void ThenMyAccountUnlockCodeHasBeenRenewed()
        {
            var user = _userReadRepository.Get(BindingData.UserEmailAddress);
            var accountUnlockCode = _tokenManager.GetTokenByKey(BindingData.AccountUnlockCodeTokenName);

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
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            user.LoginIncorrectAttempts.Should().Be(numLoginIncorrectAttempts);
        }

        [Then(@"the account unlock code and date should be set")]
        public void ThenTheAccountUnlockCodeAndDateShouldBeSet()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            user.AccountUnlockCode.Should().NotBeNullOrWhiteSpace();
            user.AccountUnlockCodeExpiry.Should().HaveValue();
        }

        [Then(@"the account unlock code and date should not be set")]
        public void ThenTheAccountUnlockCodeAndDateShouldNotBeSet()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            user.AccountUnlockCode.Should().BeNullOrWhiteSpace();
            user.AccountUnlockCodeExpiry.Should().NotHaveValue();
        }

        [Then(@"the password reset code and date should be set")]
        public void ThenThePasswordResetCodeAndDateShouldBeSet()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            user.PasswordResetCode.Should().NotBeNullOrWhiteSpace();
            user.PasswordResetCodeExpiry.Should().HaveValue();
        }

        [Then(@"the password reset code and date should not be set")]
        public void ThenThePasswordResetCodeAndDateShouldNotBeSet()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            user.PasswordResetCode.Should().BeNullOrWhiteSpace();
            user.PasswordResetCodeExpiry.Should().NotHaveValue();
        }

        [Then(@"I get the account unlock code")]
        public void ThenIGetTheAccountUnlockCode()
        {
            //_tokenManager.SetToken(AccountUnlockCodeTokenName, AccountUnlockCode);
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(BindingData.AccountUnlockCodeTokenName, user.AccountUnlockCode);
            }
        }

        [Then(@"I get the same account unlock code")]
        public void ThenIGetTheSameAccountUnlockCode()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);
            var accountUnlockCode = _tokenManager.GetTokenByKey(BindingData.AccountUnlockCodeTokenName);

            // Ensure account unlock code has changed.
            accountUnlockCode.Should().NotBeNull();
            user.AccountUnlockCode.Should().NotBeNull();
            accountUnlockCode.Should().Be(user.AccountUnlockCode);
        }

        [Then(@"I get the password reset code")]
        public void ThenIGetThePasswordResetCode()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(BindingData.PasswordResetCodeTokenName, user.PasswordResetCode);
            }
        }

        private void SetTokens(Candidate candidate)
        {
            // Email.
            _tokenManager.SetToken(BindingData.UserEmailAddressTokenName, candidate.RegistrationDetails.EmailAddress);

            // Password.
            _tokenManager.SetToken(BindingData.PasswordTokenName, BindingData.Password);
            _tokenManager.SetToken(BindingData.InvalidPasswordTokenName, BindingData.InvalidPassword);

            // Activation, unlock codes etc.
            _tokenManager.SetToken(BindingData.ActivationCodeTokenName, BindingData.ActivationCode);
            _tokenManager.SetToken(BindingData.AccountUnlockCodeTokenName, BindingData.AccountUnlockCode);
            _tokenManager.SetToken(BindingData.InvalidEmailTokenName, BindingData.InvalidEmail);
        }
    }
}