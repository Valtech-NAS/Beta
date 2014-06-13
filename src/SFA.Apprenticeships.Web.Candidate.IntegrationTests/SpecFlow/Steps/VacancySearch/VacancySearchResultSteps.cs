
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
        public void WhenISeeMyFirstSearchResults(int count)
        {
            ThenIExpectToSeeSearchResults();
            CheckNavigationLinks(count, -1, 2);
        }

        [Then(@"I expect the search results to be sorted by '(.*)'")]
        public void ThenIExpectTheSearchResultsToBeSortedBy(string sortBy)
        {
            Page.I.Assert.Value(sortBy).In("#sort-results");
        }

        [Then(@"I expect to be able to navigate to the next page of results")]
        public void ThenIExpectToBeAbleToNavigateToTheNextPageOfResults()
        {
            CheckNavigationLinks(10, 1, 2);
        }

        [When(@"I navigate to the next page of '(.*)' results")]
        public void WhenINavigateToTheNextPageOfResults(int count)
        {
            ClickLink("a.page-navigation__btn.next", string.Empty);
            ThenIExpectToSeeSearchResults();
        }

        [Then(@"I expect to see the '(.*)' page of '(.*)' results")]
        public void ThenIExpectToSeeThePageOfResults(string p0, int count)
        {
            if (p0 == "next")
            {
                CheckNavigationLinks(count, 1, 3);
            }
        }

        private void CheckNavigationLinks(int count, int prevPage, int nextPage)
        {
            Page.I.Assert.Count(count).Of("li.search-results__item");

            if (prevPage != -1)
            {
                Page.I.Assert
                    .Text(x => x.StartsWith(prevPage.ToString()))
                    .In("a.page-navigation__btn.previous span.counter");
            }

            if (nextPage != -1)
            {
                Page.I.Assert
                    .Text(x => x.StartsWith(nextPage.ToString()))
                    .In("a.page-navigation__btn.next span.counter");
            }
        }
    }
}
