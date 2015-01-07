namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class WhatHappensNextTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var mediator = GetMediator();

            var response = mediator.WhatHappensNext(Guid.NewGuid(), 1, "001", "Vacancy 001");

            response.AssertCode(Codes.ApprenticeshipApplication.WhatHappensNext.Ok, true);
        }

        private static IApprenticeshipApplicationMediator GetMediator()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel());
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(apprenticeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}