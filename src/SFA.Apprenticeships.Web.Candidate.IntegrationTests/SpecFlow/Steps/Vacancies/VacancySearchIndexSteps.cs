namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Vacancies
{
    using Common;
    using FluentAssertions;
    using FluentAutomation;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancySearchIndexSteps : CommonSteps<VacancySearchIndexPage>
    {
        private readonly VacancySearchIndexPage _vacancySearchIndexPage;

        public VacancySearchIndexSteps(FluentTest test, VacancySearchIndexPage vacancySearchIndexPage): base(test)
        {
            _vacancySearchIndexPage = vacancySearchIndexPage;
        }

        [Given(@"I am a candidate with preferences")]
        public void GivenIAmACandidateWithPreferences(Table table)
        {
            _vacancySearchIndexPage.Verify();

            table.RowCount.Should().Be(1);
            EnterCandidateCriteria(table.Rows[0]["Location"], table.Rows[0]["Distance"]);
        }

        [Given(@"I am a candidate searching for '(.*)' with a radius of '(.*)'")]
        public void GivenIAmACandidateSearchingForWithARadiusOf(string searchLocation, string searchRadius)
        {
            _vacancySearchIndexPage.Go();
            _vacancySearchIndexPage.Verify();
            EnterCandidateCriteria(searchLocation, searchRadius);
        }

        [When(@"I search for vacancies")]
        [Given(@"I have searched for vacancies")]
        public void WhenISearchForvacancies()
        {
            ClickButton("Search");
        }

        [Then(@"I expect to see a validation message")]
        public void ThenIExpectToSeeAValidationMessage(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I enhance my search with the following '(.*)'")]
        public void GivenIEnhanceMySearchWithTheFollowing(string keywords)
        {
            I.Enter(keywords).In("#Keywords");
        }

        [Then(@"I expect to see the search page")]
        public void ThenIExpectToSeeTheSearchPage()
        {
            _vacancySearchIndexPage.Verify();
        }

        [Then(@"all search fields are reset")]
        public void ThenAllSearchFieldsAreReset()
        {
            I.Assert.Text(string.Empty).In("#Keywords");
            I.Assert.Text(string.Empty).In("#Location");
            I.Assert.Value("2").In("#loc-within");
        }

        public void EnterCandidateCriteria(string location, string range)
        {
            I.Enter(location).In("#Location")
                .Wait(10)
                .Click("ul.ui-autocomplete li.ui-menu-item:first")
                .Select(range).From("#loc-within");
        }

        [Then(@"I expect no search results to be returned")]
        public void ThenIExpectNoSearchResultsToBeReturned()
        {
            I.Assert.Text("There are currently no apprenticeships that match your search.").In("#search-no-results-title");
        }

        [Then(@"I expect the sort dropdown to be removed")]
        public void ThenIExpectTheSortDropdownToBeRemoved()
        {
            I.Assert.Not.Exists("#sort-results");
        }
    }
}
