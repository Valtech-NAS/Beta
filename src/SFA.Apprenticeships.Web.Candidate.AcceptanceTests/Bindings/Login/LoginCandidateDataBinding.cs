namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Login
{
    using System;
    using System.Linq;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Generators;
    using global::SpecBind.Helpers;
    using NUnit.Framework;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class LoginCandidateDataBinding
    {
        private const string Id = "00000000-0000-0000-0000-000000000001"; // CRITICAL: must match an AD user name.
        private const string EmailAddressTokenName = "EmailAddressToken";
        private const string InvalidEmailTokenName = "InvalidEmailToken";
        private const string InvalidEmail = "invalid@gmail.com";
        private const string PasswordTokenName = "PasswordToken";
        private const string Password = "?Password01!";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";
        private const string ActivationCodeTokenName = "ActivationCodeToken";
        private const string ActivationCode = "ACTIV1";
        private const string AccountUnlockCodeTokenName = "AccountUnlockCodeToken";
        private const string AccountUnlockCode = "UNLCK1";

        private readonly string _emailAddress;

        private readonly ITokenManager _tokenManager;

        private readonly IUserReadRepository _userReadRepository;

        public LoginCandidateDataBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _emailAddress = EmailGenerator.GenerateEmailAddress();
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
        }

        //TODO: create a mechanism where we won't need to login - just get the webdriver and set the auth cookie directly or similar
        [Given("I registered an account and activated it")]
        public void GivenIRegisteredAnAccountAndActivatedIt()
        {
            var candidate = new CandidateBuilder(_emailAddress)
                .Build();

            var user = new UserBuilder(Id, _emailAddress)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I registered an account but did not activate it")]
        public void GivenIRegisteredAnAccountButDidNotActivateIt()
        {
            var candidate = new CandidateBuilder(_emailAddress)
                .Build();

            var user = new UserBuilder(Id, _emailAddress, UserStatuses.PendingActivation)
                .WithActivationCode(ActivationCode)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I made two unsuccessful login attempts")]
        public void GivenIMadeTwoUnsuccessfulLoginAttempts()
        {
            var candidate = new CandidateBuilder(_emailAddress)
                .Build();

            var user = new UserBuilder(Id, _emailAddress)
                .WithLoginIncorrectAttempts(2)
                .Build();

            SetTokens(candidate, user);
        }

        [Given("I locked my account")]
        public void GivenILockedMyAccount()
        {
            var candidate = new CandidateBuilder(_emailAddress, Id)
                .Build();

            var user = new UserBuilder(Id, _emailAddress, UserStatuses.Locked)
                .WithAccountUnlockCodeExpiry(DateTime.Now.AddDays(1))
                .WithAccountUnlockCode(AccountUnlockCode)
                .Build();

            SetTokens(candidate, user);
        }


        [Given("I locked my account and my account unlock code has expired")]
        public void GivenILockedMyAccountAndMyAccountUnlockCodeHasExpired()
        {
            var candidate = new CandidateBuilder(_emailAddress)
                .Build();

            var user = new UserBuilder(Id, _emailAddress, UserStatuses.Locked)
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
            var user = _userReadRepository.Get(_emailAddress);
            var accountUnlockCode = _tokenManager.GetTokenByKey(AccountUnlockCodeTokenName);

            // Ensure account unlock code has changed.
            accountUnlockCode.Should().NotBeNull();
            user.AccountUnlockCode.Should().NotBeNull();
            accountUnlockCode.Should().NotBe(user.AccountUnlockCode);

            // Ensure account unlock code has been renewed.
            user.AccountUnlockCodeExpiry.Should().BeAfter(DateTime.Now);
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