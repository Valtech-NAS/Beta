using SFA.Apprenticeships.Web.Candidate.UnitTests.Builders;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Applications;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ArchiveTests
    {
        [Test]
        public void SuccessTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModelBuilder().Build();

            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.ArchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();
            var response = accountMediator.Archive(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Archive.SuccessfullyArchived);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationArchived);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void ErrorTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModelBuilder().HasError("Has error").Build();

            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.ArchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();
            var response = accountMediator.Archive(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Archive.ErrorArchiving);
            response.Message.Text.Should().Be("Has error");
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }
    }
}