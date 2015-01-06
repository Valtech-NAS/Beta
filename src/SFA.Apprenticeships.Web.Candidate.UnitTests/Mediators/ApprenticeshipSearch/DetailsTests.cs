﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DetailsTests : TestsBase
    {
        private const int Id = 1;

        [Test]
        public void VacancyNotFound()
        {
            var mediator = GetMediator(null);

            var response = mediator.Details(Id, null, null);

            response.AssertCode(Codes.ApprenticeshipSearch.Details.VacancyNotFound, false);
        }

        [Test]
        public void VacancyHasError()
        {
            const string message = "The vacancy has an error";
            
            var vacancyDetailViewModel = new VacancyDetailViewModel {ViewModelMessage = message};
            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(Id, null, null);

            response.AssertMessage(Codes.ApprenticeshipSearch.Details.VacancyHasError, message, UserMessageLevel.Warning, true);
        }

        [Test]
        public void Ok()
        {
            var vacancyDetailViewModel = new VacancyDetailViewModel();
            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(Id, null, null);

            response.AssertCode(Codes.ApprenticeshipSearch.Details.Ok, true);
        }

        private static IApprenticeshipSearchMediator GetMediator(VacancyDetailViewModel vacancyDetailViewModel)
        {
            var configurationManager = new Mock<IConfigurationManager>();
            var searchProvider = new Mock<ISearchProvider>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            apprenticeshipVacancyDetailProvider.Setup(p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(configurationManager.Object, searchProvider.Object, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);
            return mediator;
        }
    }
}