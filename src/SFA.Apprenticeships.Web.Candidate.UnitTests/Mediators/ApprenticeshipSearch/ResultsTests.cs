namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResultsTests : TestsBase
    {
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<IUserDataProvider> _userDataProvider;
        private IApprenticeshipSearchMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _configurationManager = new Mock<IConfigurationManager>();
            _searchProvider = GetSearchProvider();
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            _userDataProvider = new Mock<IUserDataProvider>();
            _mediator = GetMediator(_configurationManager.Object, _searchProvider.Object, _apprenticeshipVacancyDetailProvider.Object, _userDataProvider.Object);
        }

        private static Mock<ISearchProvider> GetSearchProvider()
        {
            var searchProvider = new Mock<ISearchProvider>();
            searchProvider.Setup(sp => sp.FindLocation(It.IsAny<string>())).Returns<string>(l => new LocationsViewModel(new[] { new LocationViewModel { Name = l } }));
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

        [Test]
        public void LocationOk()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London"
            };

            var response = _mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNullOrEmpty();
            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(1);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.VacancySearch.ApprenticeshipLevel.Should().Be("All");
        }

        [Test]
        public void ApprenticeshipLevelOk()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London",
                ApprenticeshipLevel = "Higher"
            };

            var response = _mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNullOrEmpty();
            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(1);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.VacancySearch.ApprenticeshipLevel.Should().Be("Higher");
            _userDataProvider.Verify(udp => udp.Push(UserDataItemNames.ApprenticeshipLevel, "Higher"), Times.Once);
        }

        [Test]
        public void NoResults()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "Middle of Nowhere"
            };

            var response = _mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNull();
            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(0);
        }
        
        [Test]
        public void NoSearchParameters()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = string.Empty
            };

            var response = _mediator.Results(searchViewModel);

            response.AssertValidationResult(Codes.ApprenticeshipSearch.Results.ValidationError, true);
        }

        [Test]
        public void LocationSortTypes()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London"
            };

            var response = _mediator.Results(searchViewModel);

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
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London",
                Keywords = "Sales"
            };

            var response = _mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            var sortTypes = viewModel.SortTypes.ToList();
            sortTypes.Count.Should().Be(3);
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.Relevancy.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.ClosingDate.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.Distance.ToString());
        }
    }
}