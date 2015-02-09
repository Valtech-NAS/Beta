namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Locations;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsTests
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
        public void SaveValidationErrorTest()
        {
            var settingsViewModel = new SettingsViewModel();

            var response = _accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.Settings.ValidationError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.ValidationResult.Should().NotBeNull();
        }

        [Test]
        public void SaveSuccessTest()
        {
            var settingsViewModel = new SettingsViewModel
            {
                Address = new AddressViewModel
                {
                    AddressLine1 = "Add1",
                    AddressLine2 = "Add2",
                    AddressLine3 = "Add3",
                    AddressLine4 = "Add4",
                    Postcode = "N7 8LS"
                },
                DateOfBirth = new DateViewModel { Day = DateTime.Now.Day, Month = DateTime.Now.Month, Year = DateTime.Now.Year },
                PhoneNumber = "079824524523",
                Firstname = "FN",
                Lastname = "LN"
            };

            _accountProviderMock.Setup(x => x.SaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>())).Returns(true);

            var response = _accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.Settings.Success);
            response.ViewModel.Should().Be(settingsViewModel);
        }

        [Test]
        public void SaveErrorTest()
        {
            var settingsViewModel = new SettingsViewModel
            {
                Address = new AddressViewModel
                {
                    AddressLine1 = "Add1",
                    AddressLine2 = "Add2",
                    AddressLine3 = "Add3",
                    AddressLine4 = "Add4",
                    Postcode = "N7 8LS"
                },
                DateOfBirth = new DateViewModel { Day = DateTime.Now.Day, Month = DateTime.Now.Month, Year = DateTime.Now.Year },
                PhoneNumber = "079824524523",
                Firstname = "FN",
                Lastname = "LN"
            };

            _accountProviderMock.Setup(x => x.SaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>())).Returns(false);

            var response = _accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.Settings.SaveError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.Message.Text.Should().Be(AccountPageMessages.SettingsUpdateFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void SuccessTest()
        {
            _accountProviderMock.Setup(x => x.GetSettingsViewModel(It.IsAny<Guid>())).Returns(new SettingsViewModel());
            var response = _accountMediator.Settings(Guid.NewGuid());
            response.AssertCode(AccountMediatorCodes.Settings.Success);
        }
    }
}