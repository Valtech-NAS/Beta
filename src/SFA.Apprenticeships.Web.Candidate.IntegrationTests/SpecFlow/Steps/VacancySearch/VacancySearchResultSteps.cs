
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.VacancySearch
{
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages;
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Common;
    using Specflow.FluentAutomation.Ext;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancySearchResultSteps : CommonSteps
    {
        [Then(@"I expect to see search results")]
        public void ThenIExpectToSeeSearchResults()
        {
            Page = Pages.Get<VacancySearchResultPage>();
            Page.Verify();
        }

        [When(@"I see my first '(.*)' search results")]
        [Given(@"I see my first '(.*)' search results")]
        public void WhenISeeMyFirstSearchResults(int count)
        {
            ThenIExpectToSeeSearchResults();
            CheckNavigationLinks(count, 1);
        }

        [Then(@"I expect the search results to be sorted by '(.*)'")]
        public void ThenIExpectTheSearchResultsToBeSortedBy(string sortBy)
        {
            Page.I.Assert.Value(sortBy).In("#sort-results");
        }

        [Then(@"I expect to be able to navigate to the next page of results")]
        public void ThenIExpectToBeAbleToNavigateToTheNextPageOfResults()
        {
            CheckNavigationLinks(10, 1);
        }

        [Given(@"I have paged through the next '(.*)' pages")]
        [When(@"I have paged through the next '(.*)' pages")]
        public void GivenIHavePagedThroughTheNextPages(int pages)
        {
            for (var i = 0; i < pages; i++)
            {
                WhenINavigateToTheNextPageOfResults(-1);
            }
        }

        [When(@"I have paged through the previous '(.*)' pages")]
        public void WhenIHavePagedThroughThePreviousPages(int pages)
        {
            for (var i = 0; i < pages; i++)
            {
                WhenINavigateToThePreviousPageOfResults(-1);
            }
        }

        [Then(@"I expect to see the results for page '(.*)'")]
        public void ThenIExpectToSeeTheResultsForPage(int pageNumber)
        {
            CheckNavigationLinks(-1, pageNumber);
        }

        private void WhenINavigateToTheNextPageOfResults(int count)
        {
            ClickLink("a.page-navigation__btn.next", string.Empty);
            ThenIExpectToSeeSearchResults();
        }

        private void WhenINavigateToThePreviousPageOfResults(int count)
        {
            ClickLink("a.page-navigation__btn.previous", string.Empty);
            ThenIExpectToSeeSearchResults();
        }

        private void CheckNavigationLinks(int count, int pageNumber)
        {
            if (count == -1)
            {
                Page.I.Assert.Exists("li.search-results__item");
            }
            else
            {
                Page.I.Assert.Count(count).Of("li.search-results__item");
            }

            if (pageNumber > 1)
            {
                Page.I.Assert
                    .Text(x => x.StartsWith((pageNumber - 1).ToString()))
                    .In("a.page-navigation__btn.previous span.counter");
            }

            Page.I.Assert
                .Text(x => x.StartsWith((pageNumber + 1).ToString()))
                .In("a.page-navigation__btn.next span.counter");
        }
    }
}
