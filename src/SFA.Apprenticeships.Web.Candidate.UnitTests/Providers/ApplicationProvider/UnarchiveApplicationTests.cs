namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Common.Models.Application;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class UnarchiveApplicationTests : ApprenticeshipApplicationProviderTestsBase
    {
        private Guid _candidateId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _candidateId = Guid.NewGuid();
        }

        [Test]
        public void GivenException_ThenFailedApplicationViewModelIsReturned()
        {
            CandidateService.Setup(cs => cs.UnarchiveApplication(_candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = ApprenticeshipApplicationProvider.UnarchiveApplication(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenSuccessfulUnarchive_ThenSuccessfulViewModelIsReturned()
        {
            var returnedViewModel = ApprenticeshipApplicationProvider.UnarchiveApplication(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}