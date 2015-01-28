namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DetailsTests : TestsBase
    {
        private const string Id = "1";
        private const string VacancyDistance = "10";

        [Test]
        public void VacancyNotFound()
        {
            var response = Mediator.Details(Id, null);

            response.AssertCode(Codes.ApprenticeshipSearch.Details.VacancyNotFound, false);
        }

        [Test]
        public void VacancyHasError()
        {
            const string message = "The vacancy has an error";
            
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                ViewModelMessage = message,
                VacancyStatus = VacancyStatuses.Live
            };

            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            
            var response = Mediator.Details(Id, null);

            response.AssertMessage(Codes.ApprenticeshipSearch.Details.VacancyHasError, message, UserMessageLevel.Warning, true);
        }

        [Test]
        public void VacancyIsUnavailable_CandidateNotLoggedIn()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            ApprenticeshipVacancyDetailProvider.Setup(
                p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var response = Mediator.Details(Id, null);

            response.AssertCode(Codes.ApprenticeshipSearch.Details.VacancyNotFound);
        }

        [Test]
        public void VacancyIsUnavailble_CandidateLoggedInButHasNeverAppliedForVacancy()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            ApprenticeshipVacancyDetailProvider.Setup(
                p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var response = Mediator.Details(Id, Guid.NewGuid());

            response.AssertCode(Codes.ApprenticeshipSearch.Details.VacancyNotFound);
        }

        [Test]
        public void VacancyIsUnavailable_CandidateLoggedInAndHasPreviouslyAppliedForVacancy()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                DateApplied = DateTime.Today.AddDays(-1),
                CandidateApplicationStatus = ApplicationStatuses.Submitted,
                VacancyStatus = VacancyStatuses.Unavailable
            };

            ApprenticeshipVacancyDetailProvider.Setup(
                p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var response = Mediator.Details(Id, Guid.NewGuid());

            response.AssertCode(Codes.ApprenticeshipSearch.Details.VacancyNotFound);
        }

        [Test]
        public void Ok()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var response = Mediator.Details(Id, null);

            response.AssertCode(Codes.ApprenticeshipSearch.Details.Ok, true);
        }

        [Test]
        public void PopulateDistance()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            ApprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            UserDataProvider.Setup(udp => udp.Pop(UserDataItemNames.VacancyDistance)).Returns(VacancyDistance);
            UserDataProvider.Setup(udp => udp.Pop(UserDataItemNames.LastViewedVacancyId)).Returns(Convert.ToString(Id));

            var response = Mediator.Details(Id, null);

            response.AssertCode(Codes.ApprenticeshipSearch.Details.Ok, true);
        }
    }
}