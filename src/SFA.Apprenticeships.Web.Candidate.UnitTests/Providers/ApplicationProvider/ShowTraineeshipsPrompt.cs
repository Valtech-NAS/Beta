namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Candidate.Providers;
    using System.Linq;

    [TestFixture]
    public class ShowTraineeshipsPrompt
    {
        private Mock<ICandidateService> _candidateService;
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<IFeatureToggle> _featureToggle;
        const int UnsuccessfulApplications = 3;
        private ApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;

        [SetUp]
        public void SetUp()
        {
            _candidateService = new Mock<ICandidateService>();
            _configurationManager = new Mock<IConfigurationManager>();
            _featureToggle = new Mock<IFeatureToggle>();


            _configurationManager.Setup(cm => cm.GetCloudAppSetting<int>("UnsuccessfulApplicationsToShowTraineeshipsPrompt"))
                .Returns(UnsuccessfulApplications);

            _featureToggle.Setup(ft => ft.IsActive(Feature.Traineeships)).Returns(true);

            _apprenticeshipApplicationProvider = new ApprenticeshipApplicationProvider(null, _candidateService.Object, null, null, _configurationManager.Object, _featureToggle.Object);
        }

        [Test]
        public void GivenAUserHasMoreThanNUnsuccessfulApplications_ShouldSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(UnsuccessfulApplications));

            _candidateService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>())).
                Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShowTraineeshipsPrompt.Should().BeTrue();
        }

        [Test]
        public void GivenAUserHasLessThanNUnsuccessfulApplications_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            const int unsuccessfulApplicationsThreshold = UnsuccessfulApplications - 1;

            _candidateService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(unsuccessfulApplicationsThreshold));

            _candidateService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>())).
                Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShowTraineeshipsPrompt.Should().BeFalse();
        }

        [Test]
        public void GivenTraineeshipsAreNotSwitchedOn_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(UnsuccessfulApplications));

            _featureToggle.Setup(ft => ft.IsActive(Feature.Traineeships)).Returns(false);

            _candidateService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>())).
                Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShowTraineeshipsPrompt.Should().BeFalse();
        }

        [Test]
        public void GivenIveAppliedForAtLeastOneTraineeship_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(UnsuccessfulApplications));
            
            _candidateService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>())).
                Returns(GetTraineeshipApplicationSummaries(1));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShowTraineeshipsPrompt.Should().BeFalse();
        }

        private static List<ApprenticeshipApplicationSummary> GetApplicationSummaries(int applicationSummariesCount)
        {
            return Enumerable.Range(1, applicationSummariesCount)
                .Select(i => new ApprenticeshipApplicationSummary {Status = ApplicationStatuses.Unsuccessful})
                .ToList();
        }

        private static List<TraineeshipApplicationSummary> GetTraineeshipApplicationSummaries(int traineeshipSummariesCount)
        {
            if (traineeshipSummariesCount == 0) return new List<TraineeshipApplicationSummary>();

            return Enumerable.Range(1, traineeshipSummariesCount)
                .Select(i => new TraineeshipApplicationSummary())
                .ToList();
        }
    }
}