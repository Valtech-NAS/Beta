namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TraineeshipVacancyDetailsTests
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
        public void VacancyStatusLiveTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            _traineeshipVacancyDetailProvider.Setup(x =>
                x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(vacancyDetailViewModel);

            var response = _accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(Codes.AccountMediator.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusExpiredTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Expired
            };

            _traineeshipVacancyDetailProvider.Setup(x =>
                x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(vacancyDetailViewModel);

            var response = _accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(Codes.AccountMediator.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusUnavailableTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            _traineeshipVacancyDetailProvider.Setup(x =>
                x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(vacancyDetailViewModel);

            var response = _accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(Codes.AccountMediator.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void VacancyNotFoundTest()
        {
            _traineeshipVacancyDetailProvider.Setup(x =>
                x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(default(VacancyDetailViewModel));

            var response = _accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(Codes.AccountMediator.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void ErrorTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                ViewModelMessage = "Has error"
            };

            _traineeshipVacancyDetailProvider.Setup(x =>
                x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(vacancyDetailViewModel);

            var response = _accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(Codes.AccountMediator.VacancyDetails.Error);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(vacancyDetailViewModel.ViewModelMessage);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}