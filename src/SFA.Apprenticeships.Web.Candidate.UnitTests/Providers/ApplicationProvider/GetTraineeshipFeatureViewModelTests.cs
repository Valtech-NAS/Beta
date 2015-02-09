namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class GetTraineeshipFeatureViewModelTests : ApprenticeshipApplicationProviderTestsBase
    {
        private Guid _candidateId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _candidateId = Guid.NewGuid();
        }

        [Test]
        public void GivenException_ThenExceptionIsRethrown()
        {
            CandidateService.Setup(cs => cs.GetApprenticeshipApplications(_candidateId)).Throws<Exception>();

            Action action = () => ApprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(_candidateId);;
            action.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenSuccess_ThenViewModelIsReturned()
        {
            CandidateService.Setup(cs => cs.GetApprenticeshipApplications(_candidateId)).Returns(new ApprenticeshipApplicationSummary[0]);
            CandidateService.Setup(cs => cs.GetTraineeshipApplications(_candidateId)).Returns(new TraineeshipApplicationSummary[0]);
            CandidateService.Setup(cs => cs.GetCandidate(_candidateId)).Returns(new CandidateBuilder().Build());

            var returnedViewModel = ApprenticeshipApplicationProvider.GetTraineeshipFeatureViewModel(_candidateId);
            returnedViewModel.Should().NotBeNull();
        }
    }
}