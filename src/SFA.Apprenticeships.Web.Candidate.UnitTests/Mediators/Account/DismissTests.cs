namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DismissTests
    {
        [Test]
        public void SuccessfullyDismissedTest()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.DismissTraineeshipPrompts(It.IsAny<Guid>())).Returns(true);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.DismissTraineeshipPrompts(Guid.NewGuid());

            response.Should().NotBeNull();
            response.Code.Should().Be(AccountMediatorCodes.DismissTraineeshipPrompts.SuccessfullyDismissed);
        }

        [Test]
        public void ErrorDismissingTest()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.DismissTraineeshipPrompts(It.IsAny<Guid>())).Returns(false);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.DismissTraineeshipPrompts(Guid.NewGuid());

            response.Should().NotBeNull();
            response.Code.Should().Be(AccountMediatorCodes.DismissTraineeshipPrompts.ErrorDismissing);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.DismissTraineeshipPromptsFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}
