namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Applications;
    using Common.Constants;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TrackTests
    {
        [Test]
        public void SuccessTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel();

            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.UnarchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Track(Guid.NewGuid(), 1);

            response.Code.Should().Be(AccountMediatorCodes.Track.SuccessfullyTracked);
            response.Message.Should().BeNull();
        }

        [Test]
        public void ErrorTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };

            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.UnarchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Track(Guid.NewGuid(), 1);

            response.Code.Should().Be(AccountMediatorCodes.Track.ErrorTracking);
            response.Message.Text.Should().Be(applicationView.ViewModelMessage);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }
    }
}