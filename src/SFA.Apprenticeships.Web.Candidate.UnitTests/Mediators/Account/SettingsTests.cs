namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Locations;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsTests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {

        }

        [Test]
        public void SaveValidationErrorTest()
        {
            var settingsViewModel = new SettingsViewModel();
            var accountMediator = new AccountMediatorBuilder().Build();

            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
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

            var candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(new Candidate());
            var candidate = new Candidate();
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.TrySaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>(), out candidate)).Returns(true);
            var accountMediator = new AccountMediatorBuilder().With(candidateServiceProviderMock).With(accountProviderMock.Object).Build();

            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
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

            Candidate candidate;
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.TrySaveSettings(It.IsAny<Guid>(), It.IsAny<SettingsViewModel>(), out candidate)).Returns(false);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.SaveSettings(Guid.NewGuid(), settingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.Settings.SaveError);
            response.ViewModel.Should().Be(settingsViewModel);
            response.Message.Text.Should().Be(AccountPageMessages.SettingsUpdateFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void SuccessTest()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.GetSettingsViewModel(It.IsAny<Guid>())).Returns(new SettingsViewModel());
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.Settings(Guid.NewGuid());
            response.AssertCode(AccountMediatorCodes.Settings.Success);
        }
    }
}