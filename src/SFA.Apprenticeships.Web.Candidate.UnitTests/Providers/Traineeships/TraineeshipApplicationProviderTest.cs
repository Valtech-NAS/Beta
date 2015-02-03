namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using System;
    using Application.Interfaces.Logging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Mapping;
    using Candidate.Providers;

    [TestFixture]
    public class TraineeshipApplicationProviderTest
    {
        [Test]
        public void WhenIGetTheApplicationViewModel_IfIveAlreadyAppliedForTheApprenticeship_IGetAViewModelWithError()
        {
            var logger = new Mock<ILogService>();
            var mapper = new Mock<IMapper>();
            var candidateService = new Mock<ICandidateService>();
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            var traineeshipApplicationProvider = new TraineeshipApplicationProvider(
                mapper.Object, candidateService.Object, traineeshipVacancyDetailProvider.Object, logger.Object);

            candidateService.Setup(cs => cs.GetTraineeshipApplication(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(new TraineeshipApplicationDetail());

            var traineeshipApplicationViewModel = traineeshipApplicationProvider.GetApplicationViewModel(
                Guid.NewGuid(), 1);

            traineeshipApplicationViewModel.HasError().Should().BeTrue();
        }
    }
}