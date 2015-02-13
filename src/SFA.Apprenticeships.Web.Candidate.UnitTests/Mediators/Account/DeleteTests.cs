namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteTests
    {
        [Test]
        public void DeleteAlreadyDeleted()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };
            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Delete.AlreadyDeleted);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationDeleted);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void DeleteErrorDeleting()
        {
            var successApplicationView = new ApprenticeshipApplicationViewModel();
            var errorApplicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Error deleting" };
            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(errorApplicationView);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Delete.ErrorDeleting);
            response.Message.Text.Should().Be("Error deleting");
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void SuccessTest()
        {
            var successApplicationView = new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new VacancyDetailViewModel
                {
                    Title = "Vac title"
                }
            };
            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Delete.SuccessfullyDeleted);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be("Vac title");
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void SuccessVacancyExpiredOrWithdrawnTest()
        {
            var successApplicationView = new ApprenticeshipApplicationViewModel
            {
                //Expired or withdrawn vacancies will no longer exist in the system. See ApprenticeshipApplicationProvider.PatchWithVacancyDetail
                VacancyDetail = null,
                ViewModelMessage = MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable,
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            };
            var apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new ApprenticeshipApplicationViewModel());
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipApplicationProviderMock).Build();

            var response = accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Delete.SuccessfullyDeletedExpiredOrWithdrawn);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationDeleted);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }
    }
}