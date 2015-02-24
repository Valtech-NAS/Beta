namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using System;
    using Application.Interfaces.Candidates;
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
    using ErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    [TestFixture]
    public class TraineeshipApplicationProviderTest
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void WhenIGetTheApplicationViewModel_IfIveAlreadyAppliedForTheApprenticeship_IGetAViewModelWithError()
        {
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new TraineeshipApplicationDetail());
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).Build();

            var traineeshipApplicationViewModel = traineeshipApplicationProvider.GetApplicationViewModel(Guid.NewGuid(), 1);

            traineeshipApplicationViewModel.HasError().Should().BeTrue();
        }
        [Test]
        public void CreateApplicationReturnsNull()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();
            
            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void CreateApplicationReturnsVacancyStatusesUnavailable()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail { VacancyStatus = VacancyStatuses.Unavailable });
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Unavailable });
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void CreateApplicationReturnsVacancyStatusesLive()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail { VacancyStatus = VacancyStatuses.Live });
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Live });
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }

        [Test]
        public void CreateApplicationReturnsExpiredOrWithdrawn()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail { Status = ApplicationStatuses.ExpiredOrWithdrawn });
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void UnhandledError()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Throws(new CustomException(ErrorCodes.ApplicationNotFoundError));
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.UnhandledError);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Error()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Throws(new Exception());
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyNotFound()
        {
            var candidateId = Guid.NewGuid();
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyStatusUnavailable()
        {
            var candidateId = Guid.NewGuid();
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel { VacancyStatus = VacancyStatuses.Unavailable });
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.TraineeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyHasError()
        {
            var candidateId = Guid.NewGuid();
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed));
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Ok()
        {
            var candidateId = Guid.NewGuid();
            var apprenticeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetTraineeshipApplication(candidateId, ValidVacancyId)).Returns((TraineeshipApplicationDetail)null);
            candidateService.Setup(cs => cs.CreateTraineeshipApplication(candidateId, ValidVacancyId)).Returns(new TraineeshipApplicationDetail());
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().With(candidateService).With(apprenticeshipVacancyDetailProvider).Build();

            var viewModel = traineeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }
    }
}