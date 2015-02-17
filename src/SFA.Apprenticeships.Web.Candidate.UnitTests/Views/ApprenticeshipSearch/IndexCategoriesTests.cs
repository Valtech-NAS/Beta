namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Linq;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    public class IndexCategoriesTests : MediatorTestsBase
    {
        [Test]
        public void SearchModeKeywordBasicVisibilityTest()
        {
            var categories = new categories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Keyword).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("categories").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [Test]
        public void SearchModeCategoryBasicVisibilityTest()
        {
            var categories = new categories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("categories").Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("category-load-failed").Should().BeNull();
        }

        [Test]
        public void SearchModeCategoryNullCategoriesVisibilityTest()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories());

            var categories = new categories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("category-load-failed").Should().NotBeNull();
        }

        [Test]
        public void CategoryListPopulation()
        {
            var categories = new categories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = categories.RenderAsHtml(searchViewModel);

            var categoryListLeft = view.GetElementbyId("category-list-left");
            categoryListLeft.Should().NotBeNull();
            categoryListLeft.ChildNodes.Count(n => n.Name == "li").Should().Be(2);
            var categoryListRight = view.GetElementbyId("category-list-right");
            categoryListRight.Should().NotBeNull();
            categoryListRight.ChildNodes.Count(n => n.Name == "li").Should().Be(1);
        }

        [Test]
        public void CategorySelected()
        {
            var categories = new categories();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            searchViewModel.Category = "3";
            var view = categories.RenderAsHtml(searchViewModel);

            view.GetElementbyId("category-" + searchViewModel.Category.ToLower()).Should().NotBeNull();
        }
    }
}