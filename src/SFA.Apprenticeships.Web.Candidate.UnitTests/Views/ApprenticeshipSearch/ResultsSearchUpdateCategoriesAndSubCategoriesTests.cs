namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Linq;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ResultsSearchUpdateCategoriesAndSubCategoriesTests : MediatorTestsBase
    {
        [Test]
        public void SearchModeKeywordBasicVisibilityTest()
        {
            var categories = new categoriesAndSubCategories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Keyword).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("categories").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [Test]
        public void SearchModeCategoryBasicVisibilityTest()
        {
            var categories = new categoriesAndSubCategories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("categories").Attributes["class"].Value.Contains(" active").Should().BeTrue();
        }

        [Test]
        public void SearchModeCategoryNullCategoriesVisibilityTest()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories());

            var categories = new categoriesAndSubCategories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("categories").Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("category-load-failed").Should().NotBeNull();
        }

        [Test]
        public void CategoryListPopulation()
        {
            var categories = new categoriesAndSubCategories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            var categoryList = view.GetElementbyId("category-list");
            categoryList.Should().NotBeNull();
            categoryList.ChildNodes.Count(n => n.Name == "li").Should().Be(3);
        }

        [Test]
        public void CategorySelected()
        {
            var categories = new categoriesAndSubCategories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            searchViewModel.Category = "3";
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("category-" + searchViewModel.Category.ToLower()).Should().NotBeNull();
        }
    }
}