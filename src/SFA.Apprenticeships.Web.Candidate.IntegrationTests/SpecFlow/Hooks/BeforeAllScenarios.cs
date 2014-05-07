namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Hooks
{
    using FluentAutomation;
    using TechTalk.SpecFlow;

    [Binding]
    public class BeforeAllScenarios
    {

        [BeforeTestRun]
        public static void Before()
        {
        }
    }
}
