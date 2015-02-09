namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitApplicationTests : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenViewModelHasError_ThenItIsReturned()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns((ApprenticeshipApplicationDetail) null);

            var returnedViewModel = ApprenticeshipApplicationProvider.SubmitApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
        }

        [Test]
        public void GivenApplicationIsInCorrectState_ThenModelIsReturnedWithThatState()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            CandidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            CandidateService.Setup(cs => cs.SubmitApplication(candidateId, ValidVacancyId)).Throws(new CustomException(ErrorCodes.ApplicationInIncorrectStateError));

            var returnedViewModel = ApprenticeshipApplicationProvider.SubmitApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.ApplicationInIncorrectState);
        }

        [Test]
        public void GivenCustomException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            CandidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            CandidateService.Setup(cs => cs.SubmitApplication(candidateId, ValidVacancyId)).Throws(new CustomException(ErrorCodes.ApplicationCreationFailed));

            var returnedViewModel = ApprenticeshipApplicationProvider.SubmitApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GivenException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            CandidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            CandidateService.Setup(cs => cs.SubmitApplication(candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = ApprenticeshipApplicationProvider.SubmitApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GivenSuccessfulSubmission_ThenSuccessfulViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            CandidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());

            var returnedViewModel = ApprenticeshipApplicationProvider.SubmitApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
        }
    }
}