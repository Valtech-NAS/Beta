namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Candidate.Providers;
    using Common.Models.Application;
    using Configuration;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetApplicationViewModel
    {
        private Mock<ICandidateService> _candidateService;
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<IFeatureToggle> _featureToggle;
        private ApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;

        [SetUp]
        public void SetUp()
        {
            _candidateService = new Mock<ICandidateService>();
            _configurationManager = new Mock<IConfigurationManager>();
            _featureToggle = new Mock<IFeatureToggle>();

            _featureToggle.Setup(ft => ft.IsActive(Feature.Traineeships)).Returns(true);

            _apprenticeshipApplicationProvider = new ApprenticeshipApplicationProvider(null, _candidateService.Object, null, null, _configurationManager.Object, _featureToggle.Object);
        }

        [Test]
        public void GetShouldNotCreate()
        {
            _apprenticeshipApplicationProvider.GetApplicationViewModel(Guid.NewGuid(), 1);

            _candidateService.Verify(cs => cs.CreateApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
            _candidateService.Verify(cs => cs.GetApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetNotFound()
        {
            var viewModel = _apprenticeshipApplicationProvider.GetApplicationViewModel(Guid.NewGuid(), 1);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelStatus.Should().Be(ApplicationViewModelStatus.ApplicationNotFound);
            viewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationNotFound);
        }
    }
}
