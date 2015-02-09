namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DismissTests
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
        public void SuccessfullyDismissedTest()
        {
            _accountProviderMock.Setup(x => x.DismissTraineeshipPrompts(It.IsAny<Guid>())).Returns(true);

            var response = _accountMediator.DismissTraineeshipPrompts(Guid.NewGuid());

            response.Should().NotBeNull();
            response.Code.Should().Be(AccountMediatorCodes.DismissTraineeshipPrompts.SuccessfullyDismissed);
        }

        [Test]
        public void ErrorDismissingTest()
        {
            _accountProviderMock.Setup(x => x.DismissTraineeshipPrompts(It.IsAny<Guid>())).Returns(false);

            var response = _accountMediator.DismissTraineeshipPrompts(Guid.NewGuid());

            response.Should().NotBeNull();
            response.Code.Should().Be(AccountMediatorCodes.DismissTraineeshipPrompts.ErrorDismissing);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.DismissTraineeshipPromptsFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}
