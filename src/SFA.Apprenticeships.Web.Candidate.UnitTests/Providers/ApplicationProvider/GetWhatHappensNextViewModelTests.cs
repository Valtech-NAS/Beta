namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using Candidate.ViewModels.VacancySearch;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class GetWhatHappensNextViewModelTests : ApprenticeshipApplicationProviderTestsBase
    {
        private Guid _candidateId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _candidateId = Guid.NewGuid();
        }

        [Test]
        public void GivenException_ThenCreateOrRetrieveApplicationFailedIsReturned()
        {
            CandidateService.Setup(cs => cs.GetApplication(_candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = ApprenticeshipApplicationProvider.GetWhatHappensNextViewModel(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenNoApplicationFound_ThenApplicationNotFoundIsReturned()
        {
            CandidateService.Setup(cs => cs.GetApplication(_candidateId, ValidVacancyId)).Returns((ApprenticeshipApplicationDetail) null);
            CandidateService.Setup(cs => cs.GetCandidate(_candidateId)).Returns(new CandidateBuilder().Build);

            var returnedViewModel = ApprenticeshipApplicationProvider.GetWhatHappensNextViewModel(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationNotFound);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenNoCandidateFound_ThenApplicationNotFoundIsReturned()
        {
            CandidateService.Setup(cs => cs.GetApplication(_candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(_candidateId, ValidVacancyId).Build);
            CandidateService.Setup(cs => cs.GetCandidate(_candidateId)).Returns((Candidate) null);

            var returnedViewModel = ApprenticeshipApplicationProvider.GetWhatHappensNextViewModel(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationNotFound);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenPatchWithVacancyDetailHasError_ThenMessageIsReturned()
        {
            CandidateService.Setup(cs => cs.GetApplication(_candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(_candidateId, ValidVacancyId).Build);
            CandidateService.Setup(cs => cs.GetCandidate(_candidateId)).Returns(new CandidateBuilder().Build);
            ApprenticeshipVacancyDetailProvider.Setup(cs => cs.GetVacancyDetailViewModel(_candidateId, ValidVacancyId)).Returns((VacancyDetailViewModel)null);
            
            var returnedViewModel = ApprenticeshipApplicationProvider.GetWhatHappensNextViewModel(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenGetWhatHappensNextViewModelIsSuccessful_ThenViewModelIsReturned()
        {
            CandidateService.Setup(cs => cs.GetApplication(_candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(_candidateId, ValidVacancyId).Build);
            CandidateService.Setup(cs => cs.GetCandidate(_candidateId)).Returns(new CandidateBuilder().Build);
            ApprenticeshipVacancyDetailProvider.Setup(cs => cs.GetVacancyDetailViewModel(_candidateId, ValidVacancyId)).Returns(new VacancyDetailViewModelBuilder().Build());
            
            var returnedViewModel = ApprenticeshipApplicationProvider.GetWhatHappensNextViewModel(_candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }
    }
}