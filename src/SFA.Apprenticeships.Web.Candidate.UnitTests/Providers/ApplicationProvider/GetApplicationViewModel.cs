namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Candidate.ViewModels.VacancySearch;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetApplicationViewModel : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GetShouldNotCreate()
        {
            ApprenticeshipApplicationProvider.GetApplicationViewModel(Guid.NewGuid(), ValidVacancyId);

            CandidateService.Verify(cs => cs.CreateApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            CandidateService.Verify(cs => cs.GetApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetNotFound()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns((ApprenticeshipApplicationDetail)null);
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.ApplicationNotFound);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationNotFound);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void ApplicationInIncorrectState()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Throws(new CustomException(ErrorCodes.EntityStateError));
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.ApplicationInIncorrectState);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationInIncorrectState);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void UnhandledError()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Throws(new CustomException(Application.Interfaces.Applications.ErrorCodes.ApplicationNotFoundError));
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.UnhandledError);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Error()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Throws(new Exception());
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyNotFound()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel) null);
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyStatusUnavailable()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel {VacancyStatus = VacancyStatuses.Unavailable});
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void PatchWithVacancyDetail_VacancyHasError()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed));
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.Unknown);
            viewModel.ViewModelMessage.Should().Be(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void Ok()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            CandidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            var viewModel = ApprenticeshipApplicationProvider.GetApplicationViewModel(candidateId, ValidVacancyId);

            viewModel.Should().NotBeNull();
            viewModel.Status.Should().Be(ApplicationStatuses.Unknown);
            viewModel.ViewModelMessage.Should().BeNullOrEmpty();
            viewModel.HasError().Should().BeFalse();
        }
    }
}
