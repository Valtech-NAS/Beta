
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Common
{
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages;
    using Specflow.FluentAutomation.Ext;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancySearchCommonSteps : CommonSteps
    {
        [Given(@"I am a candidate with preferences")]
        public void GivenIAmACandidateWithPreferences(Table table)
        {
            var page = Pages.Get<VacancySearchIndexPage>();
            SetPage(page);

            page.Go();

            page.Verify();
            EnterCandidateCriteria("Coventry", "5 miles");
        }

        [When(@"I search for vancancies")]
        [Given(@"I have searched for vancancies")]
        public void WhenISearchForVancancies()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I expect to see a validation message")]
        public void ThenIExpectToSeeAValidationMessage(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I enhance my search with the following '(.*)'")]
        public void GivenIEnhanceMySearchWithTheFollowing(string keywords)
        {
            var page = GetPage();
            page.I.Enter(keywords).In("#keywords");
        }

        public void EnterCandidateCriteria(string location, string range)
        {
            var page = GetPage();
            page.I.Enter(location).In("#location");
            page.I.Select(range).From("#loc-within");
        }
    }
}
