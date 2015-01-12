namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class WhatHappensNextTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel());
            
            var response = Mediator.WhatHappensNext(Guid.NewGuid(), 1, "001", "Vacancy 001");

            response.AssertCode(Codes.ApprenticeshipApplication.WhatHappensNext.Ok, true);
        }
    }
}