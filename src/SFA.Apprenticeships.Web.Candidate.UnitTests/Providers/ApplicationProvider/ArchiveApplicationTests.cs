namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Common.Models.Application;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ArchiveApplicationTests : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenException_ThenFailedApplicationViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.ArchiveApplication(candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = ApprenticeshipApplicationProvider.ArchiveApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenSuccessfulArchive_ThenSuccessfulViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var returnedViewModel = ApprenticeshipApplicationProvider.ArchiveApplication(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}