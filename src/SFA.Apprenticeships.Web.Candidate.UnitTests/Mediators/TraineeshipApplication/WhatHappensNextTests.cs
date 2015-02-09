namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class WhatHappensNextTests : TestsBase
    {
        [Test]
        public void VacancyNotFound()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn });
            
            var response = Mediator.WhatHappensNext(Guid.NewGuid(), 1, "001", "Vacancy 001");

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound, false);
        }

        [Test]
        public void HasError()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel("Has Error"));
            
            var response = Mediator.WhatHappensNext(Guid.NewGuid(), 1, "001", "Vacancy 001");

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok, true);
            var viewModel = response.ViewModel;
            viewModel.VacancyReference.Should().Be("001");
            viewModel.VacancyTitle.Should().Be("Vacancy 001");
        }

        [Test]
        public void Ok()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel());

            var response = Mediator.WhatHappensNext(Guid.NewGuid(), 1, "001", "Vacancy 001");

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok, true);
        }
    }
}