namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetTraineeshipFeatureViewModelTests// : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenException_ThenExceptionIsRethrown()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetApprenticeshipApplications(candidateId)).Throws<Exception>();

            Action action = () => new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).Build()
                .GetTraineeshipFeatureViewModel(candidateId);;

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenSuccess_ThenViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetApprenticeshipApplications(candidateId)).Returns(new ApprenticeshipApplicationSummary[0]);
            candidateService.Setup(cs => cs.GetTraineeshipApplications(candidateId)).Returns(new TraineeshipApplicationSummary[0]);
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build());

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).Build()
                .GetTraineeshipFeatureViewModel(candidateId);
            returnedViewModel.Should().NotBeNull();
        }
    }
}