

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Builders;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResendTests
    {
        private const string MobileNumber = "123456789";

        [TestCase(VerifyMobileState.Ok, AccountMediatorCodes.Resend.ResentSuccessfully, VerifyMobilePageMessages.MobileVerificationCodeMayHaveBeenResent, UserMessageLevel.Success)]
        [TestCase(VerifyMobileState.MobileVerificationNotRequired, AccountMediatorCodes.Resend.ResendNotRequired, VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning)]
        [TestCase(VerifyMobileState.Error, AccountMediatorCodes.Resend.Error, VerifyMobilePageMessages.MobileVerificationCodeResendFailed, UserMessageLevel.Error)]
        public void ResendTest(VerifyMobileState verifyMobileState, string accountMediatorCode, string pageMessage, UserMessageLevel userMessageLevel)
        {
            //Arrange 
            var verifyMobileViewModel = new VerifyMobileViewModelBuilder(MobileNumber, verifyMobileState).Build();

            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.SendMobileVerificationCode(It.IsAny<Guid>(), It.IsAny<VerifyMobileViewModel>())).Returns(verifyMobileViewModel);

            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            //Act
            var response = accountMediator.Resend(Guid.NewGuid(), new VerifyMobileViewModel() { MobileNumber = MobileNumber });

            //Assert
            response.Code.Should().Be(accountMediatorCode);
            response.Message.Text.Should().Be(pageMessage);
            response.Message.Level.Should().Be(userMessageLevel);
        }
    }
}
