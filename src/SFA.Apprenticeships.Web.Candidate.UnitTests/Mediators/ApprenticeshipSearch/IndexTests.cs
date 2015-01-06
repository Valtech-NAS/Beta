namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System.Web.Mvc;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests
    {
        [Test]
        public void Ok()
        {
            var mediator = GetMediator();

            var response = mediator.Index();

            response.AssertCode(Codes.ApprenticeshipSearch.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(2);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
        }

        private static IApprenticeshipSearchMediator GetMediator()
        {
            var configurationManager = new Mock<IConfigurationManager>();
            configurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);
            var searchProvider = new Mock<ISearchProvider>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(configurationManager.Object, searchProvider.Object, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);
            return mediator;
        }

        private static IApprenticeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            var mediator = new ApprenticeshipSearchMediator(configurationManager, searchProvider, apprenticeshipVacancyDetailProvider, userDataProvider, new ApprenticeshipSearchViewModelClientValidator(), new ApprenticeshipSearchViewModelLocationValidator());
            return mediator;
        }
    }
}