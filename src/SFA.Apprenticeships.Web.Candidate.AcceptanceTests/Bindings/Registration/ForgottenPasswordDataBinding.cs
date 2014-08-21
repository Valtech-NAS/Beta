namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Registration
{
    using System;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;
    using SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Generators;
    using SpecBind.Helpers;
    using StructureMap;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ForgottenPasswordDataBinding
    {
        private const string NewPasswordTokenName = "NewPasswordToken";
        private const string NewPassword = "?Password02!";

        private const string EmailAddressTokenName = "EmailAddressToken";
        private const string PasswordResetCodeTokenName = "PasswordResetCodeToken";
        private const string PasswordResetCode = "RESET1";

        private readonly IUserReadRepository _userReadRepository;
        protected readonly ITokenManager _tokenManager;

        public ForgottenPasswordDataBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();

            SetTokens();
        }

        [When("I get the token to reset the password")]
        [Then("I get the token to reset the password")]
        public void WhenIGetTokenToResetPassword()
        {
            var email = _tokenManager.GetTokenByKey(EmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(PasswordResetCodeTokenName, user.PasswordResetCode);
            }
        }

        [Then("I get the same token to reset the password")]
        public void ThenIGetTheSameTokenToResetThePassword()
        {
            var email = _tokenManager.GetTokenByKey(EmailAddressTokenName);
            var user = _userReadRepository.Get(email);
            var resetPasswordCode = _tokenManager.GetTokenByKey(PasswordResetCodeTokenName);

            // Ensure password reset code has not changed.
            resetPasswordCode.Should().NotBeNull();
            user.PasswordResetCode.Should().NotBeNull();
            resetPasswordCode.Should().BeEquivalentTo(user.PasswordResetCode);
        }

        [Then(@"I don't receive an email with the token to reset the password")]
        public void ThenIDonTReceiveAnEmailWithTheTokenToResetThePassword()
        {
            var email = _tokenManager.GetTokenByKey(EmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            user.PasswordResetCode.Should().BeNullOrEmpty();
        }

        #region Helpers

        private void SetTokens()
        {
            // Activation, unlock codes etc.
            _tokenManager.SetToken(NewPasswordTokenName, NewPassword);
        }

        #endregion
    }
}