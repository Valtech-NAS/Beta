namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.ViewModels.Login;
    using Common.Constants;
    using Common.Providers;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResendTests
    {
        [Test]
        public void ValidationError()
        {
            var viewModel = new AccountUnlockViewModel();
            var mediator = new LoginMediatorBuilder().Build();
            var response = mediator.Resend(viewModel);
            response.AssertValidationResult(LoginMediatorCodes.Resend.ValidationError);
        }

        [Test]
        public void Success()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com" };
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success, true);
            userDataProvider.Verify(x => x.Push(UserDataItemNames.EmailAddress, viewModel.EmailAddress));
        }

        [Test]
        public void UserInIncorrectState()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Send unlock code", Status = AccountUnlockState.UserInIncorrectState };
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success, true);
        }

        [Test]
        public void AccountEmailAddressOrUnlockCodeInvalid()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Unknown username=ab@cde.com", Status = AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid };
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, AccountUnlockPageMessages.AccountUnlockCodeMayHaveBeenResent, UserMessageLevel.Success, true);
        }

        [Test]
        public void Error()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Error", Status = AccountUnlockState.Error };
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResendFailed, AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning, true);
        }
    }
}