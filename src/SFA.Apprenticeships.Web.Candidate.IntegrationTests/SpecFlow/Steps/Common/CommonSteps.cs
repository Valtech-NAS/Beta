
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Common
{
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages;
    using TechTalk.SpecFlow;

    public abstract class CommonSteps
    {
        public void SetPage(FluentAutomation.PageObject page)
        {
            ScenarioContext.Current.Add("currentPageUnderTest", page);
        }

        public IPageUnderTest GetPage()
        {
            return (IPageUnderTest)ScenarioContext.Current.Get<FluentAutomation.PageObject>("currentPageUnderTest");
        }
    }
}
