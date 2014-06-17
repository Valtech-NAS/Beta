
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.VacancySearch
{
    using FluentAssertions;
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages;
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Common;
    using Specflow.FluentAutomation.Ext;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancySearchIndexSteps : CommonSteps
    {
        [Given(@"I am a candidate with preferences")]
        public void GivenIAmACandidateWithPreferences(Table table)
        {
            Page = Pages.Get<VacancySearchIndexPage>().Go();        
            Page.Verify();

            table.RowCount.Should().Be(1);
            EnterCandidateCriteria(table.Rows[0]["Location"], table.Rows[0]["Distance"]);
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
            Page.I.Enter(keywords).In("#Keywords");
        }

        [When(@"I clear my search criteria")]
        public void WhenIClearMySearchCriteria()
        {
            ClickLink("Clear");
        }

        [Then(@"I expect to see the search page")]
        public void ThenIExpectToSeeTheSearchPage()
        {
            Page.Verify();
        }

        [Then(@"all search fields are reset")]
        public void ThenAllSearchFieldsAreReset()
        {
            Page.I.Assert.Text(string.Empty).In("#Keywords");
            Page.I.Assert.Text(string.Empty).In("#Location");
            Page.I.Assert.Value("2").In("#loc-within");
        }

        public void EnterCandidateCriteria(string location, string range)
        {
            Page.I
                .Enter(location).In("#Location")
                .Wait(1)
                .Click("ul.ui-autocomplete li.ui-menu-item:first")
                .Select(range).From("#loc-within");
        }
    }
}
