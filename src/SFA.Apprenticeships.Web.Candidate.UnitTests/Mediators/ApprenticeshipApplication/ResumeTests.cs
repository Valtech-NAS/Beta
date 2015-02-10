namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResumeTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void HasError()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy not found"));
            
            var response = Mediator.Resume(Guid.NewGuid(), InvalidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.HasError, "Vacancy not found", UserMessageLevel.Warning, false);
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
            
            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Resume.Ok, false, true);
        }

        [Test]
        public void ApplicationExpired()
        {
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            });

            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.HasError, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning, false);
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

            var response = Mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Resume.HasError, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning, false);
        }
    }
}