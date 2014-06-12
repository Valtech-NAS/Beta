
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.VacancySearch
{
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancySearchResultSteps
    {
        [When(@"I see my first '(.*)' search results")]
        public void WhenISeeMyFirstSearchResults(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I expect the search results to be sorted by '(.*)'")]
        public void ThenIExpectTheSearchResultsToBeSortedBy(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I expect to be able to navigate to the next page of results")]
        public void ThenIExpectToBeAbleToNavigateToTheNextPageOfResults()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I navigate to the next page of results")]
        public void WhenINavigateToTheNextPageOfResults()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I expect to see the '(.*)' page of results")]
        public void ThenIExpectToSeeThePageOfResults(string p0)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
