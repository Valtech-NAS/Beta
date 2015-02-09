namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Common.Models.Application;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteApplicationTests
    {
        const int ValidVacancyId = 1;

        [Test]
        public void GivenException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.DeleteApplication(candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().With(candidateService)
                .Build()
                .DeleteApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenSuccessfulDeleteApplication_ThenSuccessfulViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().Build().DeleteApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenEntityStateError_ThenDefaultViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.DeleteApplication(candidateId, ValidVacancyId)).Throws(new CustomException(Domain.Entities.ErrorCodes.EntityStateError));

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().With(candidateService)
                .Build()
                .DeleteApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenApplicationNotFoundError_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.DeleteApplication(candidateId, ValidVacancyId)).Throws(new CustomException(Application.Interfaces.Applications.ErrorCodes.ApplicationNotFoundError));

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().With(candidateService)
                .Build()
                .DeleteApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}