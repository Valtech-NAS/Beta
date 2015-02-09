namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteTests
    {
        private AccountMediator _accountMediator;
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock;
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider;
        private Mock<IAccountProvider> _accountProviderMock;
        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock;
        private Mock<IConfigurationManager> _configurationManagerMock;
        private SettingsViewModelServerValidator _settingsViewModelServerValidator;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            _traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            _accountProviderMock = new Mock<IAccountProvider>();
            _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
            _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            _configurationManagerMock = new Mock<IConfigurationManager>();

            _accountMediator = new AccountMediator(
                _accountProviderMock.Object,
                _candidateServiceProviderMock.Object,
                _settingsViewModelServerValidator,
                _apprenticeshipApplicationProviderMock.Object,
                _apprenticeshipVacancyDetailProvider.Object,
                _traineeshipVacancyDetailProvider.Object,
                _configurationManagerMock.Object);
        }

        [Test]
        public void DeleteAlreadyDeleted()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Delete.AlreadyDeleted);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationDeleted);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void DeleteErrorDeleting()
        {
            var successApplicationView = new ApprenticeshipApplicationViewModel();
            var errorApplicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Error deleting" };
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            _apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(errorApplicationView);

            var response = _accountMediator.Delete(Guid.NewGuid(), 1);
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
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            _apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);

            var response = _accountMediator.Delete(Guid.NewGuid(), 1);
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
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            _apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new ApprenticeshipApplicationViewModel());

            var response = _accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Delete.SuccessfullyDeletedExpiredOrWithdrawn);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationDeleted);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }
    }
}