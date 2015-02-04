namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Registration
{
    using Domain.Interfaces.Repositories;
    using IoC;
    using SpecBind.Helpers;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ForgottenPasswordDataBinding
    {
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
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(BindingData.PasswordResetCodeTokenName, user.PasswordResetCode);
            }
        }

        [Then("I get the same token to reset the password")]
        public void ThenIGetTheSameTokenToResetThePassword()
        {
            var email = _tokenManager.GetTokenByKey(BindingData.UserEmailAddressTokenName);
            var user = _userReadRepository.Get(email);
            var resetPasswordCode = _tokenManager.GetTokenByKey(BindingData.PasswordResetCodeTokenName);

            // Ensure password reset code has not changed.
            resetPasswordCode.Should().NotBeNull();
            user.PasswordResetCode.Should().NotBeNull();
            resetPasswordCode.Should().BeEquivalentTo(user.PasswordResetCode);
        }

        [Then(@"I don't receive an email with the token to reset the password")]
        public void ThenIDonTReceiveAnEmailWithTheTokenToResetThePassword()
        {
            var user = _userReadRepository.Get(BindingData.UserEmailAddress);

            user.PasswordResetCode.Should().BeNullOrEmpty();
        }

        private void SetTokens()
        {
            // Activation, unlock codes etc.
            _tokenManager.SetToken(BindingData.NewPasswordTokenName, BindingData.NewPassword);
        }
    }
}