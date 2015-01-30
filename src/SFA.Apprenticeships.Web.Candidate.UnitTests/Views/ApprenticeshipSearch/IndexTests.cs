namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Application.Interfaces.Vacancies;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class IndexTests
    {
        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void SearchModeKeywordShouldNotShowCategory()
        {
            var index = new Index();

            var searchViewModel = GetSearchViewModel(ApprenticeshipSearchMode.Keyword);
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").Should().NotBeNull();
            view.GetElementbyId("categories").Should().BeNull();
            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
        }

        [Test]
        public void SearchModeCategoryShouldNotShowKeywords()
        {
            var index = new Index();

            var searchViewModel = GetSearchViewModel(ApprenticeshipSearchMode.Category);
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").Should().NotBeNull();
            view.GetElementbyId("categories").Should().BeNull();
            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
        }

        private static ApprenticeshipSearchViewModel GetSearchViewModel(ApprenticeshipSearchMode searchMode)
        {
            var distances = GetDistances();
            var sortTypes = GetSortTypes();
            var apprenticeshipLevels = GetApprenticeshipLevels();
            var categories = GetCategories();

            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                WithinDistance = 5,
                LocationType = ApprenticeshipLocationType.NonNational,
                Distances = distances,
                SortTypes = sortTypes,
                ResultsPerPage = 5,
                ApprenticeshipLevels = apprenticeshipLevels,
                ApprenticeshipLevel = "All",
                Categories = categories.ToList(),
                SearchMode = searchMode
            };

            return searchViewModel;
        }

        private static SelectList GetDistances()
        {
            var distances = new SelectList(
                new[]
                {
                    new {WithinDistance = 2, Name = "2 miles"},
                    new {WithinDistance = 5, Name = "5 miles"},
                    new {WithinDistance = 10, Name = "10 miles"},
                    new {WithinDistance = 15, Name = "15 miles"},
                    new {WithinDistance = 20, Name = "20 miles"},
                    new {WithinDistance = 30, Name = "30 miles"},
                    new {WithinDistance = 40, Name = "40 miles"}
                },
                "WithinDistance",
                "Name"
                );

            return distances;
        }

        private static SelectList GetSortTypes(VacancySearchSortType selectedSortType = VacancySearchSortType.Distance, string keywords = null, bool isLocalLocationType = true)
        {
            var sortTypeOptions = new ArrayList();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sortTypeOptions.Add(new { SortType = VacancySearchSortType.Relevancy, Name = "Best Match" });
            }

            sortTypeOptions.Add(new { SortType = VacancySearchSortType.ClosingDate, Name = "Closing Date" });

            if (isLocalLocationType)
            {
                sortTypeOptions.Add(new { SortType = VacancySearchSortType.Distance, Name = "Distance" });
            }

            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
                );

            return sortTypes;
        }

        private static SelectList GetResultsPerPageSelectList(int selectedValue)
        {
            var resultsPerPage = new SelectList(
                new[]
                {
                    new {ResultsPerPage = 5, Name = "5 per page"},
                    new {ResultsPerPage = 10, Name = "10 per page"},
                    new {ResultsPerPage = 25, Name = "25 per page"},
                    new {ResultsPerPage = 50, Name = "50 per page"}
                },
                "ResultsPerPage",
                "Name",
                selectedValue
                );

            return resultsPerPage;
        }

        private static SelectList GetApprenticeshipLevels(string selectedValue = "All")
        {
            var apprenticeshipLevels = new SelectList(
                new[]
                {
                    new {ApprenticeshipLevel = "All", Name = "All levels"},
                    new {ApprenticeshipLevel = "Intermediate", Name = "Intermediate"},
                    new {ApprenticeshipLevel = "Advanced", Name = "Advanced"},
                    new {ApprenticeshipLevel = "Higher", Name = "Higher"}
                },
                "ApprenticeshipLevel",
                "Name",
                selectedValue
                );

            return apprenticeshipLevels;
        }

        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
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
                    CodeName = "2",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "2_1"},
                        new Category {CodeName = "2_2"},
                        new Category {CodeName = "2_3"}
                    }
                },
                new Category
                {
                    CodeName = "3",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "3_1"}
                    }
                }
            };
        }
    }
}