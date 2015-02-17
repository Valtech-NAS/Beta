namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ResultsSearchUpdateTests : MediatorTestsBase
    {
        [Test]
        public void SearchModeKeywordBasicVisibilityTest()
        {
            var searchUpdate = new searchUpdate();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Keyword).ViewModel;
            var searchResultsViewModel = Mediator.Results(searchViewModel).ViewModel;
            var view = searchUpdate.RenderAsHtml(searchResultsViewModel.VacancySearch);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
        }

        [Test]
        public void SearchModeCategoryBasicVisibilityTest()
        {
            var searchUpdate = new searchUpdate();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var searchResultsViewModel = Mediator.Results(searchViewModel).ViewModel;
            var view = searchUpdate.RenderAsHtml(searchResultsViewModel.VacancySearch);

            view.GetElementbyId("validation-summary").Should().BeNull();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
        }

        /// <summary>
        /// Form fields should no longer be visible and a message should be present
        /// to tell users loading of categories prevents filering by them
        /// </summary>
        [Test]
        public void SearchModeCategoryNullCategoryiesVisibilityTest()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories());
            var searchUpdate = new searchUpdate();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var searchResultsViewModel = Mediator.Results(searchViewModel).ViewModel;
            var view = searchUpdate.RenderAsHtml(searchResultsViewModel.VacancySearch);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("apprenticeship-level").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();
            view.GetElementbyId("search-button").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }
    }
}