namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using System.Collections.Generic;
    using Candidate.Mediators;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests
    {
        private AccountMediator _accountMediator;
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock;
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider;
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
            _traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            _accountProviderMock = new Mock<IAccountProvider>();
            _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
            _emptyMyApplicationsView = new MyApplicationsViewModel(new List<MyApprenticeshipApplicationViewModel>(), new List<MyTraineeshipApplicationViewModel>(), new TraineeshipFeatureViewModel());
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
            _apprenticeshipApplicationProviderMock.Setup(x => x.GetMyApplications(It.IsAny<Guid>())).Returns(_emptyMyApplicationsView);

            var response = _accountMediator.Index(Guid.NewGuid(), "deletedVacancyId", "deletedVacancyTitle");
            response.Code.Should().Be(Codes.AccountMediator.Index.Success);
            response.ViewModel.Should().Be(_emptyMyApplicationsView);
            response.ViewModel.DeletedVacancyId.Should().Be("deletedVacancyId");
            response.ViewModel.DeletedVacancyTitle.Should().Be("deletedVacancyTitle");
        }
    }
}