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
        private const int SomeVacancyId = 1;
        private const string VacancyReference = "001";
        private Guid _someCandidateId;
        private const string VacancyTitle = "Vacancy 001";

        [SetUp]
        public void SetUp()
        {
            _someCandidateId = Guid.NewGuid();
        }

        [Test]
        public void VacancyNotFound()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn });

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId.ToString(), VacancyReference, VacancyTitle);

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound, false);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" 491802")]
        [TestCase("VAC000547307")]
        [TestCase("[[imgUrl]]")]
        [TestCase("separator.png")]
        public void GivenInvalidVacancyIdString_ThenVacancyNotFound(string vacancyId)
        {
            var response = Mediator.WhatHappensNext(_someCandidateId, vacancyId, VacancyReference, VacancyTitle);

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound, false);
        }

        [Test]
        public void HasError()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel("Has Error"));

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId.ToString(), VacancyReference, VacancyTitle);

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok, true);
            var viewModel = response.ViewModel;
            viewModel.VacancyReference.Should().Be(VacancyReference);
            viewModel.VacancyTitle.Should().Be(VacancyTitle);
        }

        [Test]
        public void Ok()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(new WhatHappensNextViewModel());

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId.ToString(), VacancyReference, VacancyTitle);

            response.AssertCode(TraineeshipApplicationMediatorCodes.WhatHappensNext.Ok, true);
        }
    }
}