namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Common.Models.Application;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteApplicationTests : ApprenticeshipApplicationProviderTestsBase
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
            CandidateService.Setup(cs => cs.DeleteApplication(_candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = ApprenticeshipApplicationProvider.DeleteApplication(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenSuccessfulDeleteApplication_ThenSuccessfulViewModelIsReturned()
        {
            var returnedViewModel = ApprenticeshipApplicationProvider.DeleteApplication(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenEntityStateError_ThenDefaultViewModelIsReturned()
        {
            CandidateService.Setup(cs => cs.DeleteApplication(_candidateId, ValidVacancyId)).Throws(new CustomException(Domain.Entities.ErrorCodes.EntityStateError));

            var returnedViewModel = ApprenticeshipApplicationProvider.DeleteApplication(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Ok);
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenApplicationNotFoundError_ThenFailedApplicationViewModelIsReturned()
        {
            CandidateService.Setup(cs => cs.DeleteApplication(_candidateId, ValidVacancyId)).Throws(new CustomException(Application.Interfaces.Applications.ErrorCodes.ApplicationNotFoundError));

            var returnedViewModel = ApprenticeshipApplicationProvider.DeleteApplication(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.Error);
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}