namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Registration
{
    using Domain.Interfaces.Repositories;
    using IoC;
    using SpecBind.Helpers;
    using StructureMap;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ForgottenPasswordDataBinding
    {
        private const string UserEmailAddress = "nas.exemplar+acceptancetests@gmail.com";
        private const string NewPasswordTokenName = "NewPasswordToken";
        private const string NewPassword = "?Password02!";

        private const string EmailTokenName = "EmailToken";
        private const string PasswordResetCodeTokenName = "PasswordResetCodeToken";

        private readonly IUserReadRepository _userReadRepository;
        private readonly ITokenManager _tokenManager;

        public ForgottenPasswordDataBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _userReadRepository = WebTestRegistry.Container.GetInstance<IUserReadRepository>();

            SetTokens();
        }

        [When("I get the token to reset the password")]
        [Then("I get the token to reset the password")]
        public void WhenIGetTokenToResetPassword()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(PasswordResetCodeTokenName, user.PasswordResetCode);
            }
        }

        [Then("I get the same token to reset the password")]
        public void ThenIGetTheSameTokenToResetThePassword()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenName);
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
            var user = _userReadRepository.Get(UserEmailAddress);

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