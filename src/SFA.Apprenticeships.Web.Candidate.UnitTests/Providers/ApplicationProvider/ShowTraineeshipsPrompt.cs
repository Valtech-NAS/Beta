namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Candidates;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Web.Candidate.Providers;
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
            _candidateService.Setup(cs => cs.GetApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(UnsuccessfulApplications));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShouldShowTraineeshipsPrompt.Should().BeTrue();
        }

        [Test]
        public void GivenAUserHasLessThanNUnsuccessfulApplications_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            const int unsuccessfulApplicationsThreshold = UnsuccessfulApplications - 1;

            _candidateService.Setup(cs => cs.GetApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(unsuccessfulApplicationsThreshold));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShouldShowTraineeshipsPrompt.Should().BeFalse();
        }

        [Test]
        public void GivenTraineeshipsAreNotSwitchedOn_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateService.Setup(cs => cs.GetApplications(It.IsAny<Guid>())).
                Returns(GetApplicationSummaries(UnsuccessfulApplications));

            _featureToggle.Setup(ft => ft.IsActive(Feature.Traineeships)).Returns(false);


            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.ShouldShowTraineeshipsPrompt.Should().BeFalse();
        }

        private static List<ApprenticeshipApplicationSummary> GetApplicationSummaries(int applicationSummariesCount)
        {
            return Enumerable.Range(1, applicationSummariesCount)
                .Select(i => new ApprenticeshipApplicationSummary {Status = ApplicationStatuses.Unsuccessful})
                .ToList();
        }
    }
}