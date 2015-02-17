namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class IndexTests : MediatorTestsBase
    {
        [Test]
        public void SearchModeKeywordBasicVisibilityTest()
        {
            var index = new Index();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Keyword).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();


            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeTrue();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }

        [Test]
        public void SearchModeCategoryBasicVisibilityTest()
        {
            var index = new Index();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            
            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            
            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeTrue();
        }

        /// <summary>
        /// Form fields should no longer be visible and a message should be present
        /// to tell users loading of categories prevents filering by them
        /// </summary>
        [Test]
        public void SearchModeCategoryNullCategoriesVisibilityTest()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories());
            var index = new Index();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();
            
            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" tab2").Should().BeFalse();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();
        }
    }
}