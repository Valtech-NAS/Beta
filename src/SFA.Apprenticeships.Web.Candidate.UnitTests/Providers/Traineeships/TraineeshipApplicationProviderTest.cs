namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using System;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Candidates;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Providers;

    [TestFixture]
    public class TraineeshipApplicationProviderTest
    {
        [Test]
        public void WhenIGetTheApplicationViewModel_IfIveAlreadyAppliedForTheApprenticeship_IGetAViewModelWithError()
        {
            var mapper = new Mock<IMapper>();
            var candidateService = new Mock<ICandidateService>();
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            var traineeshipApplicationProvider = new TraineeshipApplicationProvider(
                mapper.Object, candidateService.Object, traineeshipVacancyDetailProvider.Object);

            candidateService.Setup(cs => cs.GetTraineeshipApplication(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(new TraineeshipApplicationDetail());

            var traineeshipApplicationViewModel = traineeshipApplicationProvider.GetApplicationViewModel(
                Guid.NewGuid(), 1);

            traineeshipApplicationViewModel.HasError().Should().BeTrue();
        }
    }
}