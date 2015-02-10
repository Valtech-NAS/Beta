namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Domain.Entities.Applications;

    [TestFixture]
    public class WhatHappensNextTests : TestsBase
    {
        private const int SomeVacancyId = 1;
        private const string VacancyReference = "001";
        private Guid _someCandidateId;
        private const string SomeErrorMessage = "Error message";
        private const string VacancyTitle = "Vacancy 001";

        [SetUp]
        public void SetUp()
        {
            _someCandidateId = Guid.NewGuid();
        }

        [Test]
        public void Ok()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(_someCandidateId, SomeVacancyId)).Returns(new WhatHappensNextViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            });

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId.ToString(), VacancyReference, VacancyTitle);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.Ok, true);
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

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound, false);
        }

        [Test]
        public void ExpiredOrWithdrawnVacancyReturnsAVacancyNotFound()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(_someCandidateId, SomeVacancyId)).Returns(new WhatHappensNextViewModel
            {
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            });

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId.ToString(), VacancyReference, VacancyTitle);

            response.Code.Should().Be(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.VacancyNotFound);
        }

        [Test]
        public void IfModelHasError_PopulateVacancyTitleAndVacancyReferenceInTheModel()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(_someCandidateId, SomeVacancyId)).Returns(new WhatHappensNextViewModel(SomeErrorMessage));

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId.ToString(), VacancyReference, VacancyTitle);
            response.AssertCode(ApprenticeshipApplicationMediatorCodes.WhatHappensNext.Ok, true);
            response.ViewModel.VacancyTitle = VacancyTitle;
            response.ViewModel.VacancyReference = VacancyReference;
        }
    }
}