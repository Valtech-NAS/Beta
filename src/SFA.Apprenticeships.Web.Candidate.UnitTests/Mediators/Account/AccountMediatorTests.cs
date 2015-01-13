namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using System.Collections.Generic;
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
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AccountMediatorTests
    {
        private AccountMediator _accountMediator;
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock;
        private Mock<IAccountProvider> _accountProviderMock;
        private SettingsViewModelServerValidator _settingsViewModelServerValidator;
        private MyApplicationsViewModel _emptyMyApplicationsView;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            _accountProviderMock = new Mock<IAccountProvider>();
            _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
            _emptyMyApplicationsView = new MyApplicationsViewModel(new List<MyApprenticeshipApplicationViewModel>(),new List<MyTraineeshipApplicationViewModel>(), new TraineeshipPromptViewModel());
            _accountMediator = new AccountMediator(_accountProviderMock.Object, _settingsViewModelServerValidator, _apprenticeshipApplicationProviderMock.Object);
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
                ViewModelMessage = MyApplicationsPageMessages.DraftExpired,
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
    }
}
