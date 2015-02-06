namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using Domain.Entities.Applications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplyTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void VacancyNotFound()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn });
            
            var response = Mediator.Apply(Guid.NewGuid(), InvalidVacancyId.ToString());

            response.AssertCode(Codes.ApprenticeshipApplication.Apply.VacancyNotFound, false);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" 491802")]
        [TestCase("VAC000547307")]
        [TestCase("[[imgUrl]]")]
        public void GivenInvalidVacancyIdString_ThenVacancyNotFound(string vacancyId)
        {
            var response = Mediator.Apply(Guid.NewGuid(), vacancyId);

            response.AssertCode(Codes.ApprenticeshipApplication.Apply.VacancyNotFound, false);
        }

        [Test]
        public void HasError()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy has error"));

            var response = Mediator.Apply(Guid.NewGuid(), InvalidVacancyId.ToString());

            response.AssertCode(Codes.ApprenticeshipApplication.Apply.HasError, false);
        }

        [Test]
        public void Ok()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel());

            var response = Mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(Codes.ApprenticeshipApplication.Apply.Ok, true);
        }
    }
}