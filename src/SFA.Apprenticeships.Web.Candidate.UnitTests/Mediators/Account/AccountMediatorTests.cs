namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Locations;
    using Candidate.ViewModels.MyApplications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AccountMediatorTests
    {
        private AccountMediator _accountMediator;
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock;
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        // TODO: AG: US680: private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider;
        private Mock<IAccountProvider> _accountProviderMock;
        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock;
        private Mock<IConfigurationManager> _configurationManagerMock; 
        private SettingsViewModelServerValidator _settingsViewModelServerValidator;
        private MyApplicationsViewModel _emptyMyApplicationsView;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();

            _accountProviderMock = new Mock<IAccountProvider>();
            _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
            _emptyMyApplicationsView = new MyApplicationsViewModel(new List<MyApprenticeshipApplicationViewModel>(),new List<MyTraineeshipApplicationViewModel>(), new TraineeshipFeatureViewModel());
            _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            _configurationManagerMock = new Mock<IConfigurationManager>();
            
            _accountMediator = new AccountMediator(
                _accountProviderMock.Object,
                _candidateServiceProviderMock.Object,
                _settingsViewModelServerValidator,
                _apprenticeshipApplicationProviderMock.Object,
                _apprenticeshipVacancyDetailProvider.Object,
                _configurationManagerMock.Object);
        }

        [Test]
        public void IndexSuccessTest()
        {
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetMyApplications(It.IsAny<Guid>())).Returns(_emptyMyApplicationsView);
            
            var response = _accountMediator.Index(Guid.NewGuid(), "deletedVacancyId", "deletedVacancyTitle");
            response.Code.Should().Be(Codes.AccountMediator.Index.Success);
            response.ViewModel.Should().Be(_emptyMyApplicationsView);
            response.ViewModel.DeletedVacancyId.Should().Be("deletedVacancyId");
            response.ViewModel.DeletedVacancyTitle.Should().Be("deletedVacancyTitle");
        }

        [Test]
        public void ArchiveSuccessTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel();
            _apprenticeshipApplicationProviderMock.Setup(x => x.ArchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Archive(Guid.NewGuid(), 1);
            response.Code.Should().Be(Codes.AccountMediator.Archive.SuccessfullyArchived);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationArchived);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void ArchiveErrorTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };
            _apprenticeshipApplicationProviderMock.Setup(x => x.ArchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Archive(Guid.NewGuid(), 1);
            response.Code.Should().Be(Codes.AccountMediator.Archive.ErrorArchiving);
            response.Message.Text.Should().Be("Has error");
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void DeleteAlreadyDeleted()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);
            
            var response = _accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(Codes.AccountMediator.Delete.AlreadyDeleted);
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
            response.Code.Should().Be(Codes.AccountMediator.Delete.ErrorDeleting);
            response.Message.Text.Should().Be("Error deleting");
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void DeleteSuccess()
        {
            var successApplicationView = new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new VacancyDetailViewModel()
                {
                    Title = "Vac title"
                }
            };
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);
            _apprenticeshipApplicationProviderMock.Setup(x => x.DeleteApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(successApplicationView);

            var response = _accountMediator.Delete(Guid.NewGuid(), 1);
            response.Code.Should().Be(Codes.AccountMediator.Delete.SuccessfullyDeleted);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be("Vac title");
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void DeleteSuccessVacancyExpiredOrWithdrawn()
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
            response.Code.Should().Be(Codes.AccountMediator.Delete.SuccessfullyDeletedExpiredOrWithdrawn);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApplicationDeleted);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }


        [Test]
        public void SettingsSaveValidationErrorTest()
        {
            var settingsViewModel = new SettingsViewModel();
            
            var response = _accountMediator.Settings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(Codes.AccountMediator.Settings.ValidationError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.ValidationResult.Should().NotBeNull();
        }

        [Test]
        public void SettingsSaveSuccessTest()
        {
            var settingsViewModel = new SettingsViewModel()
            {
                Address = new AddressViewModel()
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

            var response = _accountMediator.Settings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(Codes.AccountMediator.Settings.Success);
            response.ViewModel.Should().Be(settingsViewModel);
        }

        [Test]
        public void SettingsSaveErrorTest()
        {
            var settingsViewModel = new SettingsViewModel()
            {
                Address = new AddressViewModel()
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

            var response = _accountMediator.Settings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(Codes.AccountMediator.Settings.SaveError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.Message.Text.Should().Be(AccountPageMessages.SettingsUpdateFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void SettingsSuccessTest()
        {
            _accountProviderMock.Setup(x => x.GetSettingsViewModel(It.IsAny<Guid>())).Returns(new SettingsViewModel());
            var response = _accountMediator.Settings(Guid.NewGuid());
            response.AssertCode(Codes.AccountMediator.Settings.Success);
        }

        [Test]
        public void DismissTraineeshipPromptTest_SuccessfullyDismissed()
        {
            _accountProviderMock.Setup(x => x.DismissTraineeshipPrompts(It.IsAny<Guid>())).Returns(true);

            var response = _accountMediator.DismissTraineeshipPrompts(Guid.NewGuid());

            response.Should().NotBeNull();
            response.Code.Should().Be(Codes.AccountMediator.DismissTraineeshipPrompts.SuccessfullyDismissed);
        }

        [Test]
        public void DismissTraineeshipPromptTest_ErrorDismissing()
        {
            _accountProviderMock.Setup(x => x.DismissTraineeshipPrompts(It.IsAny<Guid>())).Returns(false);

            var response = _accountMediator.DismissTraineeshipPrompts(Guid.NewGuid());

            response.Should().NotBeNull();
            response.Code.Should().Be(Codes.AccountMediator.DismissTraineeshipPrompts.ErrorDismissing);
            response.Message.Text.Should().Be(MyApplicationsPageMessages.DismissTraineeshipPromptsFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }

        [Test]
        public void TrackSuccessTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel();

            _apprenticeshipApplicationProviderMock.Setup(x => x.UnarchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Track(Guid.NewGuid(), 1);

            response.Code.Should().Be(Codes.AccountMediator.Track.SuccessfullyTracked);
            response.Message.Should().BeNull();
        }

        [Test]
        public void TrackErrorTest()
        {
            var applicationView = new ApprenticeshipApplicationViewModel { ViewModelMessage = "Has error" };

            _apprenticeshipApplicationProviderMock.Setup(x => x.UnarchiveApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(applicationView);

            var response = _accountMediator.Track(Guid.NewGuid(), 1);

            response.Code.Should().Be(Codes.AccountMediator.Track.ErrorTracking);
            response.Message.Text.Should().Be(applicationView.ViewModelMessage);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [TestCase("1")]
        [TestCase(null)]
        public void AcceptTermsAndConditionsSuccessTest(string version)
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = version }
            };
            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            _candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.SuccessfullyAccepted);
        }

        [Test]
        public void AcceptTermsAndConditionsAlreadyAcceptedTest()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = "1.1" }
            };

            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            _candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);

            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.AlreadyAccepted);
        }

        [Test]
        public void AcceptTermsAndConditionsErrorTest()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = "1" }
            };

            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            _candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(false);

            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.ErrorAccepting);
        }

        [Test]
        public void AcceptTermsAndConditionsExceptionTest()
        {
            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Throws<Exception>();
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.ErrorAccepting);
        }

        [Test]
        public void ApprenticeshipDetailsLiveVacancyTest()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            _apprenticeshipVacancyDetailProvider.Setup(x =>
                x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(vacancyDetailViewModel);

            var response = _accountMediator.ApprenticeshipVacancyDetails(42);

            response.Code.Should().Be(Codes.AccountMediator.VacancyDetails.Available);
        }
    }
}
