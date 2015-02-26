namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Builders;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SearchResultsTests
    {
        [TestCase(5)]
        [TestCase(6)]
        public void NonNationalResultsPerPageDropDown(int totalLocalHits)
        {
            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder().WithTotalLocalHits(totalLocalHits).Build();
            var result = new SearchResultsViewBuilder().With(viewModel).Render();

            var resultsPerPageDropDown = result.GetElementbyId("results-per-page");

            if (totalLocalHits > 5)
            {
                resultsPerPageDropDown.Should().NotBeNull();
            }
            else
            {
                resultsPerPageDropDown.Should().BeNull();
            }
        }

        [TestCase(5)]
        [TestCase(6)]
        public void NationalResultsPerPageDropDown(int totalNationalHits)
        {
            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder().WithTotalNationalHits(totalNationalHits).Build();
            var result = new SearchResultsViewBuilder().With(viewModel).Render();

            var resultsPerPageDropDown = result.GetElementbyId("results-per-page");

            if (totalNationalHits > 5)
            {
                resultsPerPageDropDown.Should().NotBeNull();
            }
            else
            {
                resultsPerPageDropDown.Should().BeNull();
            }
        }
    }
}