namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Entities.Applications;

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
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(_someCandidateId, SomeVacancyId)).Returns(new WhatHappensNextViewModel());

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId, VacancyReference, VacancyTitle);

            response.AssertCode(Codes.ApprenticeshipApplication.WhatHappensNext.Ok, true);
        }

        [Test]
        public void ExpiredOrWithdrawnVacancyReturnsAVacancyNotFound()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(_someCandidateId, SomeVacancyId)).Returns(new WhatHappensNextViewModel
            {
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            });

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId, VacancyReference, VacancyTitle);

            response.Code.Should().Be(Codes.ApprenticeshipApplication.WhatHappensNext.VacancyNotFound);
        }

        [Test]
        public void IfModelHasError_PopulateVacancyTitleAndVacancyReferenceInTheModel()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetWhatHappensNextViewModel(_someCandidateId, SomeVacancyId)).Returns(new WhatHappensNextViewModel(SomeErrorMessage));

            var response = Mediator.WhatHappensNext(_someCandidateId, SomeVacancyId, VacancyReference, VacancyTitle);
            response.AssertCode(Codes.ApprenticeshipApplication.WhatHappensNext.Ok, true);
            response.ViewModel.VacancyTitle = VacancyTitle;
            response.ViewModel.VacancyReference = VacancyReference;
        }
    }
}