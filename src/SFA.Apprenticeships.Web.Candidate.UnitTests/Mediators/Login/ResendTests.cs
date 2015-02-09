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

            response.AssertMessage(LoginMediatorCodes.Resend.ResentSuccessfully, string.Format(AccountUnlockPageMessages.AccountUnlockCodeResent, viewModel.EmailAddress), UserMessageLevel.Success, true);
            UserDataProvider.Verify(x => x.Push(UserDataItemNames.EmailAddress, viewModel.EmailAddress));
        }

        [Test]
        public void Fail()
        {
            var viewModel = new AccountUnlockViewModel { EmailAddress = "ab@cde.com", ViewModelMessage = "Error" };
            CandidateServiceProvider.Setup(x => x.RequestAccountUnlockCode(It.IsAny<AccountUnlockViewModel>())).Returns(viewModel);
            UserDataProvider.Setup(x => x.Push(It.IsAny<string>(), It.IsAny<string>()));

            var response = Mediator.Resend(viewModel);

            response.AssertMessage(LoginMediatorCodes.Resend.ResendFailed, AccountUnlockPageMessages.AccountUnlockResendCodeFailed, UserMessageLevel.Warning, true);
        }
    }
}