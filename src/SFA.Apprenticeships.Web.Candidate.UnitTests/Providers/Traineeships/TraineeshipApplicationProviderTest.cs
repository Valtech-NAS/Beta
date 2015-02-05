namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
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
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TraineeshipApplicationProviderTest
    {
        private const int ValidVacancyId = 1;

        private Mock<ITraineeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<ICandidateService> _candidateService;
        private Mock<ILogService> _logService;
        private TraineeshipApplicationProvider _traineeshipApplicationProvider;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            _candidateService = new Mock<ICandidateService>();
            _logService = new Mock<ILogService>();

            _traineeshipApplicationProvider = new TraineeshipApplicationProvider(new TraineeshipCandidateWebMappers(), _candidateService.Object, _apprenticeshipVacancyDetailProvider.Object, _logService.Object);
        }

        [Test]
        public void WhenIGetTheApplicationViewModel_IfIveAlreadyAppliedForTheApprenticeship_IGetAViewModelWithError()
        {
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new TraineeshipApplicationDetail());

            var traineeshipApplicationViewModel = _traineeshipApplicationProvider.GetApplicationViewModel(Guid.NewGuid(), 1);

            traineeshipApplicationViewModel.HasError().Should().BeTrue();
        }
        [Test]
        public void CreateApplicationReturnsNull()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail) null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void CreateApplicationReturnsVacancyStatusesUnavailable()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail { VacancyStatus = VacancyStatuses.Unavailable });
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Unavailable });
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void CreateApplicationReturnsVacancyStatusesLive()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail { VacancyStatus = VacancyStatuses.Live });
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Live });
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }

        [Test]
        public void CreateApplicationReturnsExpiredOrWithdrawn()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail { Status = ApplicationStatuses.ExpiredOrWithdrawn });
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void UnhandledError()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Throws(new CustomException(Application.Interfaces.Applications.ErrorCodes.ApplicationNotFoundError));
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.UnhandledError);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Error()
        {
            var candidateId = Guid.NewGuid();
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Throws(new Exception());
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyNotFound()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyStatusUnavailable()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Unavailable });
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyHasError()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed));
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Ok()
        {
            var candidateId = Guid.NewGuid();
            _apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            _candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            _candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var viewModel = _traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }
    }
}