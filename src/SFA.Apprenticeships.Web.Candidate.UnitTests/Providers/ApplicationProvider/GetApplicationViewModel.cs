﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Providers;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetApplicationViewModel
    {
        private Mock<ILogService> _logService;
        private Mock<ICandidateService> _candidateService;
        private Mock<IConfigurationManager> _configurationManager;
        private ApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;

        [SetUp]
        public void SetUp()
        {
            _logService = new Mock<ILogService>();
            _candidateService = new Mock<ICandidateService>();
            _configurationManager = new Mock<IConfigurationManager>();

            _apprenticeshipApplicationProvider = new ApprenticeshipApplicationProvider(null, _candidateService.Object,
                null, _configurationManager.Object, _logService.Object);
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
