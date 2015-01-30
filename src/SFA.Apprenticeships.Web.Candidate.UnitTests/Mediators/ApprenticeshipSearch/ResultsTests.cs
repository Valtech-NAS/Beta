namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Domain.Entities.Vacancies.Apprenticeships;

    [TestFixture]
    public class ResultsTests : TestsBase
    {
        private const string AKeyword = "A keyword";
        private const string ACityWithOneSuggestedLocation = "London";
        private const string ACityWithoutSuggestedLocations = "Liverpool";
        private const string SomeErrorMessage = "SomeErrorMessage";
        private const string ACityWithMoreThanOneSuggestedLocation = "Manchester";

        private ApprenticeshipSearchViewModel _searchSentToSearchProvider;

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
            SearchProvider.Setup(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchViewModel>())).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = emptyVacancies, VacancySearch = svm }).Callback<ApprenticeshipSearchViewModel>(svm => { _searchSentToSearchProvider = svm; });
            SearchProvider.Setup(sp => sp.FindVacancies(It.Is<ApprenticeshipSearchViewModel>(svm => svm.Location == ACityWithOneSuggestedLocation))).Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel { Vacancies = londonVacancies, VacancySearch = svm }).Callback<ApprenticeshipSearchViewModel>(svm => { _searchSentToSearchProvider = svm; });
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

            SearchProvider.Setup(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchViewModel>()))
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

        [Test]
        public void ExactMatchFoundUsingVacancyReference()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = "VAC000123456"
            };

            SearchProvider.Setup(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchViewModel>()))
                .Returns(new ApprenticeshipSearchResponseViewModel
                {
                    ExactMatchFound = true,
                    VacancySearch = searchViewModel,
                    Vacancies = new List<ApprenticeshipVacancySummaryViewModel>
                    {
                        new ApprenticeshipVacancySummaryViewModel
                        {
                            Id = 123456
                        }
                    }
                });

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.ExactMatchFound, false, true);
        }

        [Test]
        public void NewSearchWithKeywords()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                SortType = VacancySortType.Distance,
                SearchAction = SearchAction.Search,
                Keywords = AKeyword
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.Relevancy);
        }

        [Test]
        public void SortWithKeywords()
        {
            const VacancySortType originalSortType = VacancySortType.Distance;

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                SortType = originalSortType,
                SearchAction = SearchAction.Sort,
                Keywords = AKeyword
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(originalSortType);
        }

        [Test]
        public void ChangeLocationTypeOnNonNationalSearchWithKeywords()
        {
            const VacancySortType originalSortType = VacancySortType.Distance;

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                SortType = originalSortType,
                SearchAction = SearchAction.LocationTypeChanged,
                Keywords = AKeyword
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.Relevancy);
        }

        [Test]
        public void ChangeLocationTypeOnNonNationalSearchWithoutKeywords()
        {
            const VacancySortType originalSortType = VacancySortType.Distance;

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                SortType = originalSortType,
                SearchAction = SearchAction.LocationTypeChanged
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.Distance);
        }

        [Test]
        public void ChangeLocationTypeOnNationalSearchWithKeywords()
        {
            const VacancySortType originalSortType = VacancySortType.Distance;

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.National,
                SortType = originalSortType,
                SearchAction = SearchAction.LocationTypeChanged,
                Keywords = AKeyword
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.Relevancy);
        }

        [Test]
        public void ChangeLocationTypeOnNationalSearchWithoutKeywords()
        {
            const VacancySortType originalSortType = VacancySortType.Distance;

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.National,
                SortType = originalSortType,
                SearchAction = SearchAction.LocationTypeChanged
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.ClosingDate);
        }

        [Test]
        public void TestCategorySearchModification()
        {
            const string selectedCategoryCode = "2";
            const string selectedCategorySubCategory = "2_2";

            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(new List<Category>
            {
                new Category
                {
                    CodeName = "1",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "1_1"},
                        new Category {CodeName = "1_2"}
                    }
                },
                new Category
                {
                    CodeName = selectedCategoryCode,
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "2_1"},
                        new Category {CodeName = selectedCategorySubCategory}
                    }
                }
            });

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = AKeyword,
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                Category = selectedCategoryCode,
                //Select Sub Categories from a different category than the one selected plus a valid one
                SubCategories = new[]
                {
                    "1_1",
                    "1_2",
                    selectedCategorySubCategory
                },
                SearchMode = ApprenticeshipSearchMode.Category
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            //The search sent to the search provider should have been modified based on the search mode
            _searchSentToSearchProvider.Should().NotBeNull();
            _searchSentToSearchProvider.Keywords.Should().BeNullOrEmpty();
            _searchSentToSearchProvider.Location.Should().Be(ACityWithOneSuggestedLocation);
            _searchSentToSearchProvider.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            _searchSentToSearchProvider.Categories.Should().NotBeNull();
            _searchSentToSearchProvider.Categories.Count.Should().Be(2);
            _searchSentToSearchProvider.Category.Should().Be(selectedCategoryCode);
            _searchSentToSearchProvider.SubCategories.Length.Should().Be(1);
            _searchSentToSearchProvider.SubCategories[0].Should().Be(selectedCategorySubCategory);
            _searchSentToSearchProvider.SearchMode.Should().Be(ApprenticeshipSearchMode.Category);
            
            //But the returned search should be the original search the user submitted so as not to lose any of their changes
            var returnedSearch = response.ViewModel.VacancySearch;
            returnedSearch.Should().NotBeNull();
            returnedSearch.Keywords.Should().Be(AKeyword);
            returnedSearch.Location.Should().Be(ACityWithOneSuggestedLocation);
            returnedSearch.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            returnedSearch.Categories.Should().NotBeNull();
            returnedSearch.Categories.Count.Should().Be(2);
            returnedSearch.Category.Should().Be(selectedCategoryCode);
            returnedSearch.SubCategories.Length.Should().Be(1);
            returnedSearch.SubCategories[0].Should().Be(selectedCategorySubCategory);
            returnedSearch.SearchMode.Should().Be(ApprenticeshipSearchMode.Category);
        }

        [Test]
        public void TestKeywordSearchModification()
        {
            const string selectedCategoryCode = "2";
            const string selectedCategorySubCategory = "2_2";

            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(new List<Category>
            {
                new Category
                {
                    CodeName = "1",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "1_1"},
                        new Category {CodeName = "1_2"}
                    }
                },
                new Category
                {
                    CodeName = selectedCategoryCode,
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "2_1"},
                        new Category {CodeName = selectedCategorySubCategory}
                    }
                }
            });

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = AKeyword,
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                Category = selectedCategoryCode,
                //Select Sub Categories from a different category than the one selected plus a valid one
                SubCategories = new[]
                {
                    "1_1",
                    "1_2",
                    selectedCategorySubCategory
                },
                SearchMode = ApprenticeshipSearchMode.Keyword
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            //The search sent to the search provider should have been modified based on the search mode
            _searchSentToSearchProvider.Should().NotBeNull();
            _searchSentToSearchProvider.Keywords.Should().Be(AKeyword);
            _searchSentToSearchProvider.Location.Should().Be(ACityWithOneSuggestedLocation);
            _searchSentToSearchProvider.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            _searchSentToSearchProvider.Categories.Should().BeNull();
            _searchSentToSearchProvider.Category.Should().BeNullOrEmpty();
            _searchSentToSearchProvider.SubCategories.Should().BeNull();
            _searchSentToSearchProvider.SearchMode.Should().Be(ApprenticeshipSearchMode.Keyword);
            
            //But the returned search should be the original search the user submitted so as not to lose any of their changes
            var returnedSearch = response.ViewModel.VacancySearch;
            returnedSearch.Should().NotBeNull();
            returnedSearch.Keywords.Should().Be(AKeyword);
            returnedSearch.Location.Should().Be(ACityWithOneSuggestedLocation);
            returnedSearch.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            returnedSearch.Categories.Should().NotBeNull();
            returnedSearch.Categories.Count.Should().Be(2);
            returnedSearch.Category.Should().Be(selectedCategoryCode);
            returnedSearch.SubCategories.Length.Should().Be(1);
            returnedSearch.SubCategories[0].Should().Be(selectedCategorySubCategory);
            returnedSearch.SearchMode.Should().Be(ApprenticeshipSearchMode.Keyword);
        }

        [Test]
        public void CategorySearchWithKeywordsShouldNotShowBestMatchOption()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = AKeyword,
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                Category = "1",
                SearchMode = ApprenticeshipSearchMode.Category
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.SortType.Should().Be(VacancySortType.Distance);
            var sortTypes = response.ViewModel.SortTypes.ToList();
            sortTypes.Count.Should().Be(2);
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.ClosingDate.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySortType.Distance.ToString());
        }

        [Test]
        public void LocationTypeShouldBeCopiedOver()
        {
            SearchProvider.Setup(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchViewModel>()))
                .Callback<ApprenticeshipSearchViewModel>(svm =>
                {
                    svm.LocationType = ApprenticeshipLocationType.National;
                    _searchSentToSearchProvider = svm;
                })
                .Returns<ApprenticeshipSearchViewModel>(svm => new ApprenticeshipSearchResponseViewModel {Vacancies = new ApprenticeshipVacancySummaryViewModel[0], VacancySearch = svm});

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = AKeyword,
                Location = ACityWithOneSuggestedLocation,
                LocationType = ApprenticeshipLocationType.NonNational,
                Category = "1",
                SearchMode = ApprenticeshipSearchMode.Category
            };

            var response = Mediator.Results(searchViewModel);

            response.AssertCode(Codes.ApprenticeshipSearch.Results.Ok, true);

            response.ViewModel.VacancySearch.LocationType.Should().Be(ApprenticeshipLocationType.National);
        }
    }
}