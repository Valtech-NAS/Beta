namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    [TestFixture]
    public class SubmitApplicationTests
    {
        const int ValidVacancyId = 1;

        [Test]
        public void GivenViewModelHasError_ThenItIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns((ApprenticeshipApplicationDetail) null);

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).Build()
                .SubmitApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.ExpiredOrWithdrawn);
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
        }

        [Test]
        public void GivenApplicationIsInCorrectState_ThenModelIsReturnedWithThatState()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();

            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            candidateService.Setup(cs => cs.SubmitApplication(candidateId, ValidVacancyId)).Throws(new CustomException(ErrorCodes.ApplicationInIncorrectStateError));

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).With(apprenticeshipVacancyDetailProvider).Build()
                .SubmitApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.ApplicationInIncorrectState);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenCustomException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            candidateService.Setup(cs => cs.SubmitApplication(candidateId, ValidVacancyId)).Throws(new CustomException(ErrorCodes.ApplicationCreationFailed));

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(apprenticeshipVacancyDetailProvider).With(candidateService).Build()
                .SubmitApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());
            candidateService.Setup(cs => cs.SubmitApplication(candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(apprenticeshipVacancyDetailProvider).With(candidateService).Build()
                .SubmitApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenSuccessfulSubmission_ThenSuccessfulViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModel());
            candidateService.Setup(cs => cs.CreateApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetail());

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(apprenticeshipVacancyDetailProvider).With(candidateService).Build()
                .SubmitApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}