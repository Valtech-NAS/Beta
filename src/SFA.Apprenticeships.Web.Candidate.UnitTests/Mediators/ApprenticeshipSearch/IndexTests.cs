namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System.Collections.Generic;
    using System.Linq;
    using Candidate.Mediators;
    using Candidate.ViewModels.VacancySearch;
    using Constants;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [Test]
        public void Ok_Keyword()
        {
            var response = Mediator.Index(ApprenticeshipSearchMode.Keyword);

            response.AssertCode(Codes.ApprenticeshipSearch.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
            viewModel.SearchMode.Should().Be(ApprenticeshipSearchMode.Keyword);
        }

        [Test]
        public void Ok_Category()
        {
            var response = Mediator.Index(ApprenticeshipSearchMode.Category);

            response.AssertCode(Codes.ApprenticeshipSearch.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
            viewModel.SearchMode.Should().Be(ApprenticeshipSearchMode.Category);
        }

        [Test]
        public void BlacklistedCategoryCodes()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(GetCategories);

            var response = Mediator.Index(ApprenticeshipSearchMode.Category);

            var categories = response.ViewModel.Categories;
            categories.Count.Should().Be(3);
            categories.Any(c => c.CodeName == "00").Should().BeFalse();
            categories.Any(c => c.CodeName == "99").Should().BeFalse();
        }

        [Test]
        public void RememberApprenticeshipLevel()
        {
            UserDataProvider.Setup(udp => udp.Get(CandidateDataItemNames.ApprenticeshipLevel)).Returns("Advanced");

            var response = Mediator.Index(ApprenticeshipSearchMode.Keyword);

            var viewModel = response.ViewModel;
            viewModel.ApprenticeshipLevel.Should().Be("Advanced");
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
                },
                new Category
                {
                    CodeName = "00",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "00_1"}
                    }
                },
                new Category
                {
                    CodeName = "99",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "99_1"}
                    }
                }
            };
        }
    }
}