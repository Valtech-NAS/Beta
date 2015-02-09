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
        public void SuccessTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel();
            _apprenticeshipApplicationProviderMock.Setup(x => x.ArchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Archive(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Archive.SuccessfullyArchived);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationArchived);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void ErrorTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };
            _apprenticeshipApplicationProviderMock.Setup(x => x.ArchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Archive(Guid.NewGuid(), 1);
            response.Code.Should().Be(AccountMediatorCodes.Archive.ErrorArchiving);
            response.Message.Text.Should().Be("Has error");
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }
    }
}