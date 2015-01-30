namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ResultsTests : MediatorTestsBase
    {
        [Test]
        public void Category_NoResults()
        {
            var results = new Results();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            searchViewModel.Category = "1";
            var searchResultsViewModel = Mediator.Results(searchViewModel).ViewModel;
            var view = results.RenderAsHtml(searchResultsViewModel);

            view.GetElementbyId("search-no-results").Should().NotBeNull();
            view.GetElementbyId("search-no-results-category").Should().NotBeNull();
            view.GetElementbyId("search-no-results-sub-category").Should().BeNull();
        }

        [Test]
        public void SubCategory_NoResults()
        {
            var results = new Results();

            var searchViewModel = Mediator.Index(ApprenticeshipSearchMode.Category).ViewModel;
            searchViewModel.Category = "1";
            searchViewModel.SubCategories = new []{"1_1"};
            var searchResultsViewModel = Mediator.Results(searchViewModel).ViewModel;
            var view = results.RenderAsHtml(searchResultsViewModel);

            view.GetElementbyId("search-no-results").Should().NotBeNull();
            view.GetElementbyId("search-no-results-category").Should().BeNull();
            view.GetElementbyId("search-no-results-sub-category").Should().NotBeNull();
        }
    }
}