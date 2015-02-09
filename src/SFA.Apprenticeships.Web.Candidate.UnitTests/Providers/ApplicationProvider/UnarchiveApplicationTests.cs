namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Common.Models.Application;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnarchiveApplicationTests
    {
        const int ValidVacancyId = 1;

        [Test]
        public void GivenException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.UnarchiveApplication(candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).Build()
                .UnarchiveApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenSuccessfulUnarchive_ThenSuccessfulViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().Build()
                .UnarchiveApplication(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}