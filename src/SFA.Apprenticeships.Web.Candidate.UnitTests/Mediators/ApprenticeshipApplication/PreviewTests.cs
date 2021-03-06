﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
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
    public class PreviewTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void VacancyNotFound()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn, VacancyDetail = new VacancyDetailViewModel()});
            
            var response = Mediator.Preview(Guid.NewGuid(), InvalidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound, false);
        }

        [Test]
        public void HasError()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy not found"));
            
            var response = Mediator.Preview(Guid.NewGuid(), InvalidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.HasError, false);
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
            
            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.Ok, true);
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

            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.VacancyNotFound, false);
        }

        [Test]
        public void OfflineVacancy()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new VacancyDetailViewModel
                {
                    ApplyViaEmployerWebsite = true
                }
            });

            var response = Mediator.Preview(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Preview.OfflineVacancy, false);
        }
    }
}