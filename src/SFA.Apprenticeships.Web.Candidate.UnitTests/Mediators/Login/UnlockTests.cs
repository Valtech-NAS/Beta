namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.ViewModels.Login;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnlockTests : TestsBase
    {
        [Test]
        public void ValidationError()
        {
            var viewModel = new AccountUnlockViewModel();

            var response = Mediator.Unlock(viewModel);

            response.AssertValidationResult(LoginMediatorCodes.Unlock.ValidationError);
        }

        [TestCase(AccountUnlockState.Ok, LoginMediatorCodes.Unlock.UnlockedSuccessfully, null, null)]
        [TestCase(AccountUnlockState.UserInIncorrectState, LoginMediatorCodes.Unlock.UserInIncorrectState, null, null)]
        [TestCase(AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid, LoginMediatorCodes.Unlock.AccountEmailAddressOrUnlockCodeInvalid, AccountUnlockPageMessages.WrongEmailAddressOrAccountUnlockCodeErrorText, UserMessageLevel.Error)]
        [TestCase(AccountUnlockState.AccountUnlockCodeExpired, LoginMediatorCodes.Unlock.AccountUnlockCodeExpired, AccountUnlockPageMessages.AccountUnlockCodeExpired, UserMessageLevel.Warning)]
        [TestCase(AccountUnlockState.Error, LoginMediatorCodes.Unlock.AccountUnlockFailed, AccountUnlockPageMessages.AccountUnlockFailed, UserMessageLevel.Warning)]
        public void CheckAllOtherResponses(AccountUnlockState accountUnlockState, string code, string message, UserMessageLevel messageLevel)
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", AccountUnlockCode = "ABC123", Status = accountUnlockState };

            CandidateServiceProvider.Setup(x => x.VerifyAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);

            var response = Mediator.Unlock(viewModel);

            response.Code.Should().Be(code);

            if (message == null)
            {
                response.Message.Should().BeNull();
            }
            else
            {
                response.Message.Should().NotBeNull();
                response.Message.Text.Should().Be(message);
                response.Message.Level.Should().Be(messageLevel);
            }
            
        }
    }
}