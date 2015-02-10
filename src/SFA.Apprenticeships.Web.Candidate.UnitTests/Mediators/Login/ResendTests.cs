namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.ViewModels.Login;
    using Common.Constants;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResendTests : TestsBase
    {
        [Test]
        public void ValidationError()
        {
            var viewModel = new AccountUnlockViewModel();
            var response = Mediator.Resend(viewModel);
            response.AssertValidationResult(LoginMediatorCodes.Resend.ValidationError);
        }

        [Test]
        public void Success()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com" };
            CandidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            UserDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));

            var response = Mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success, true);
            UserDataProvider.Verify(x => x.Push(UserDataItemNames.EmailAddress, viewModel.EmailAddress));
        }

        [Test]
        public void UserInIncorrectState()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Send unlock code", Status = AccountUnlockState.UserInIncorrectState };
            CandidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            UserDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));

            var response = Mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success, true);
        }

        [Test]
        public void AccountEmailAddressOrUnlockCodeInvalid()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Unknown username=ab@cde.com", Status = AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid };
            CandidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            UserDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));

            var response = Mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success, true);
        }

        [Test]
        public void Error()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Error", Status = AccountUnlockState.Error };
            CandidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            UserDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));

            var response = Mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResendFailed, AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning, true);
        }
    }
}