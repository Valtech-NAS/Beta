namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResultsTests : TestsBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            
            SearchProvider.Setup(sp => sp.FindLocation(It.IsAny<string>())).Returns<string>(l => new LocationsViewModel(new[] { new LocationViewModel { Name = l } }));
            var londonVacancies = new[]
            {
                new ApprenticeshipVacancySummaryViewModel {Description = "A London Vacancy"}
            };
            var emptyVacancies = new ApprenticeshipVacancySummaryViewModel[0];
            //This order is important. Moq will run though all matches and pick the last one
            SearchProvider.Setup(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchViewModel>())).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = emptyVacancies, VacancySearch = svm });
            SearchProvider.Setup(sp => sp.FindVacancies(It.Is<ApprenticeshipSearchViewModel>(svm => svm.Location == "London"))).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = londonVacancies, VacancySearch = svm });
        }

        [Test]
        public void LocationOk()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London"
            };

            var response = Mediator.Results(searchViewModel);

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

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNullOrEmpty();
            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(1);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.VacancySearch.ApprenticeshipLevel.Should().Be("Higher");
            UserDataProvider.Verify(udp => udp.Push(UserDataItemNames.ApprenticeshipLevel, "Higher"), Times.Once);
        }

        [Test]
        public void NoResults()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "Middle of Nowhere"
            };

            var response = Mediator.Results(searchViewModel);

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

            var response = Mediator.Results(searchViewModel);

            response.AssertValidationResult(Codes.ApprenticeshipSearch.Results.ValidationError, true);
        }

        [Test]
        public void LocationSortTypes()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = "London"
            };

            var response = Mediator.Results(searchViewModel);

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

            var response = Mediator.Results(searchViewModel);

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