namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResultsTests : TestsBase
    {
        [Test]
        public void LocationOk()
        {
            var mediator = GetMediator();
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London"
            };
            
            var response = mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNullOrEmpty();
            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(1);
        }

        [Test]
        public void NoResults()
        {
            var mediator = GetMediator();
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "Middle of Nowhere"
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNull();
            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(0);
        }
        
        [Test]
        public void NoSearchParameters()
        {
            var mediator = GetMediator();
            var searchViewModel = new ApprenticeshipSearchViewModel();

            var response = mediator.Results(searchViewModel);

            response.AssertValidationResult(Codes.ApprenticeshipSearch.Results.ValidationError, true);
        }

        [Test]
        public void LocationSortTypes()
        {
            var mediator = GetMediator();
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London"
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            var sortTypes = viewModel.SortTypes.ToList();
            sortTypes.Count.Should().Be(2);
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.ClosingDate.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.Distance.ToString());
        }

        [Test]
        public void KeywordSortTypes()
        {
            var mediator = GetMediator();
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London",
                Keywords = "Sales"
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            var sortTypes = viewModel.SortTypes.ToList();
            sortTypes.Count.Should().Be(3);
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.Relevancy.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.ClosingDate.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.Distance.ToString());
        }

        private static Mock<ISearchProvider> GetSearchProvider()
        {
            var searchProvider = new Mock<ISearchProvider>();
            searchProvider.Setup(sp => sp.FindLocation(It.IsAny<string>())).Returns<string>(l => new LocationsViewModel(new[] {new LocationViewModel {Name = l}}));
            var londonVacancies = new[]
            {
                new ApprenticeshipVacancySummaryViewModel {Description = "A London Vacancy"}
            };
            var emptyVacancies = new ApprenticeshipVacancySummaryViewModel[0];
            //This order is important. Moq will run though all matches and pick the last one
            searchProvider.Setup(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchViewModel>())).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = emptyVacancies, VacancySearch = svm });
            searchProvider.Setup(sp => sp.FindVacancies(It.Is<ApprenticeshipSearchViewModel>(svm => svm.Location == "London"))).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = londonVacancies, VacancySearch = svm });
            return searchProvider;
        }

        private static IApprenticeshipSearchMediator GetMediator()
        {
            var searchProvider = GetSearchProvider();
            var mediator = GetMediator(searchProvider.Object);
            return mediator;
        }

        private static IApprenticeshipSearchMediator GetMediator(ISearchProvider searchProvider)
        {
            var configurationManager = new Mock<IConfigurationManager>();
            var apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(configurationManager.Object, searchProvider, apprenticeshipVacancyDetailProvider.Object, userDataProvider.Object);
            return mediator;
        }
    }
}