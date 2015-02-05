namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetOrCreateApplicationViewModel
    {
        private const int ValidVacancyId = 1;

        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<ICandidateService> _candidateService;
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<ILogService> _logService;
        private ApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            _candidateService = new Mock<ICandidateService>();
            _configurationManager = new Mock<IConfigurationManager>();
            _logService = new Mock<ILogService>();

            _apprenticeshipApplicationProvider = new ApprenticeshipApplicationProvider(_apprenticeshipVacancyDetailProvider.Object, _candidateService.Object, new ApprenticeshipCandidateWebMappers(), _configurationManager.Object, _logService.Object);
        }

        [Test]
        public void GetShouldCreate()
        {
            _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(Guid.NewGuid(), ValidVacancyId);

            _candidateService.Verify(cs => cs.CreateApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            _candidateService.Verify(cs => cs.GetApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void CreateApplicationReturnsNull()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns((ApprenticeshipApplicationDetail) null);
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel) null);
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void CreateApplicationReturnsVacancyStatusesUnavailable()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail { VacancyStatus = VacancyStatuses.Unavailable });
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Unavailable });
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void CreateApplicationReturnsVacancyStatusesLive()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail { VacancyStatus = VacancyStatuses.Live });
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Live });
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.Unknown);
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }

        [Test]
        public void CreateApplicationReturnsExpiredOrWithdrawn()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail {Status = ApplicationStatuses.ExpiredOrWithdrawn});
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel) null);
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void ApplicationInIncorrectState()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Throws(new CustomException(Domain.Entities.ErrorCodes.EntityStateError));
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.ApplicationInIncorrectState);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationInIncorrectState);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void UnhandledError()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Throws(new CustomException(Application.Interfaces.Applications.ErrorCodes.ApplicationNotFoundError));
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.UnhandledError);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Error()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Throws(new Exception());
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyNotFound()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel) null);
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyStatusUnavailable()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel {VacancyStatus = VacancyStatuses.Unavailable});
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyHasError()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed));
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.Unknown);
            viewModel.ViewModelMessage.Should().Be(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Ok()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            _candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = _apprenticeshipApplicationProvider.GetOrCreateApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.Unknown);
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }
    }
}
