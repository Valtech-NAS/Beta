namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

    [TestFixture]
    public class ResultsTests : TestsBase
    {
        private const string ACityWithOneSuggestedLocation = "London";
        private const string ACityWithoutSuggestedLocations = "Liverpool";
        private const string SomeErrorMessage = "SomeErrorMessage";
        private const string ACityWithMoreThanOneSuggestedLocation = "Manchester";
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
            SearchProvider.Setup(sp => sp.FindVacancies(It.Is<ApprenticeshipSearchViewModel>(svm => svm.Location == ACityWithOneSuggestedLocation))).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = londonVacancies, VacancySearch = svm });
        }

        [Test]
        public void LocationOk()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation
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
                Location = ACityWithOneSuggestedLocation,
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
                Location = ACityWithOneSuggestedLocation
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
                Location = ACityWithOneSuggestedLocation,
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

        [Test]
        public void IfTheLocationIsNationalAndThereAreNoKeywords_SortTypeShouldBeClosingDate()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.National,
                SortType = VacancySortType.Distance,
                SearchAction = SearchAction.Sort
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.ClosingDate);
        }

        [Test]
        public void ShouldShowAMessageIfAnErrorOccursWhileFindingSuggestedLocations()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithMoreThanOneSuggestedLocation
            };

            SearchProvider.Setup(sp => sp.FindLocation(ACityWithMoreThanOneSuggestedLocation))
                .Returns(() => new LocationsViewModel{ViewModelMessage = SomeErrorMessage});

            var response = Mediator.Results(searchViewModel);

            response.AssertMessage(Codes.ApprenticeshipSearch.Results.HasError, SomeErrorMessage, UserMessageLevel.Warning, true);
        }

        [Test]
        public void ShouldReturnAListOfSuggestedLocations()
        {
            const int numberOfSuggestedLocations = 4;
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithMoreThanOneSuggestedLocation
            };

            SearchProvider.Setup(sp => sp.FindLocation(ACityWithMoreThanOneSuggestedLocation))
                .Returns(() => new LocationsViewModel(Enumerable.Range(1, numberOfSuggestedLocations + 1).Select(e => new LocationViewModel { Name = Convert.ToString(e) })));

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);
            response.ViewModel.LocationSearches.Should().HaveCount(numberOfSuggestedLocations);
        }

        [Test]
        public void LocationResultIsNotValid()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithoutSuggestedLocations
            };

            SearchProvider.Setup(sp => sp.FindLocation(ACityWithoutSuggestedLocations))
                .Returns(() => new LocationsViewModel());

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);
            response.ViewModel.VacancySearch.Should().Be(searchViewModel);
        }

        [Test]
        public void ErrorFindingVacancies()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation
            };

            SearchProvider.Setup(sp => sp.FindVacancies(searchViewModel))
                .Returns(new ApprenticeshipSearchResponseViewModel
                {
                    ViewModelMessage = SomeErrorMessage
                });

            var response = Mediator.Results(searchViewModel);

            response.AssertMessage(Codes.ApprenticeshipSearch.Results.HasError, SomeErrorMessage, UserMessageLevel.Warning, true);
        }

        [Test]
        public void IfTotalLocalHitsIsGreaterThanZero_LocationTypeIsNonNational()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation
            };

            SearchProvider.Setup(sp => sp.FindVacancies(searchViewModel))
                .Returns(new ApprenticeshipSearchResponseViewModel
                {
                    TotalLocalHits = 1,
                    VacancySearch = searchViewModel
                });

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);
            response.ViewModel.VacancySearch.LocationType = ApprenticeshipLocationType.NonNational;
        }
    }
}