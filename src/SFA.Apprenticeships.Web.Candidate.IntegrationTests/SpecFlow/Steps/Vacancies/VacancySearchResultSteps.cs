namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Vacancies
{
    using System;
    using System.Linq;
    using Common;
    using FluentAssertions;
    using FluentAutomation;
    using Pages;
    using Specflow.FluentAutomation.Ext;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancySearchResultSteps : CommonSteps
    {

        public VacancySearchResultSteps()
        {
            
        }

        [When(@"I expect to see search results")]
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
            var sortValues = Page.I.FindMultiple(sortBy == "Distance" ? ".distance-value" : ".closing-date-value")().ToArray();
            sortValues.Should().NotBeNull();
            sortValues.Count().Should().Be(5);

            switch (sortBy){
                case "Distance":
                    for (int i = 0; i < sortValues.Count() - 1; i++)
                    {
                        double.Parse(sortValues[i].Text)
                            .Should()
                            .BeLessOrEqualTo(double.Parse(sortValues[i + 1].Text));
                    }
                    break;
                case "ClosingDate":
                    for (int i = 0; i < sortValues.Count() - 1; i++)
                    {
                        DateTime.Parse(sortValues[i].Text)
                            .Should()
                            .BeOnOrBefore(DateTime.Parse(sortValues[i + 1].Text));
                    }
                    break;
            }
        }

        [Given(@"I update search results to be sorted by '(.*)'")]
        public void GivenIUpdateSearchResultsToBeSortedBy(string sortBy)
        {
            Page.I.Select(Option.Value, sortBy).From("#sort-results");
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
                WhenINavigateToTheNextPageOfResults();
            }
        }

        [When(@"I have paged through the previous '(.*)' pages")]
        public void WhenIHavePagedThroughThePreviousPages(int pages)
        {
            for (var i = 0; i < pages; i++)
            {
                WhenINavigateToThePreviousPageOfResults();
            }
        }

        [Then(@"I expect to see the results for page '(.*)'")]
        public void ThenIExpectToSeeTheResultsForPage(int pageNumber)
        {
            CheckNavigationLinks(-1, pageNumber);
        }

        private void WhenINavigateToTheNextPageOfResults()
        {
            ClickLink("a.page-navigation__btn.next", string.Empty);
            ThenIExpectToSeeSearchResults();
        }

        private void WhenINavigateToThePreviousPageOfResults()
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

        [When(@"I select the result returned from position '(.*)'")]
        public void WhenISelectTheResultReturnedFromPosition(int resultIndex)
        {
            Page.I.Click(() => Page.I.FindMultiple(".vacancy-title-link:first a")().Skip(resultIndex - 1).First());
        }
    }
}
