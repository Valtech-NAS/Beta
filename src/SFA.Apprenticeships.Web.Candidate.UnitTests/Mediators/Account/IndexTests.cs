namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using System.Collections.Generic;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests
    {
        [Test]
        public void SuccessTest()
        {
            var emptyMyApplicationsView = new MyApplicationsViewModel(new List<MyApprenticeshipApplicationViewModel>(), new List<MyTraineeshipApplicationViewModel>(), new TraineeshipFeatureViewModel());
            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.GetMyApplications(It.IsAny<Guid>())).Returns(emptyMyApplicationsView);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Index(Guid.NewGuid(), "deletedVacancyId", "deletedVacancyTitle");
            response.Code.Should().Be(AccountMediatorCodes.Index.Success);
            response.ViewModel.Should().Be(emptyMyApplicationsView);
            response.ViewModel.DeletedVacancyId.Should().Be("deletedVacancyId");
            response.ViewModel.DeletedVacancyTitle.Should().Be("deletedVacancyTitle");
        }
    }
}