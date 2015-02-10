namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
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

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, false);
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
            var response = Mediator.Apply(Guid.NewGuid(), vacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, false);
        }

        [Test]
        public void HasError()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy has error"));

            var response = Mediator.Apply(Guid.NewGuid(), InvalidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.HasError, false);
        }

        [Test]
        public void Ok()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            });

            var response = Mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.Ok, true);
        }

        [Test]
        public void VacancyExpired()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Expired
                }
            });

            var response = Mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, false);
        }
    }
}