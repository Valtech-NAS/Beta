namespace SFA.Apprenticeships.Tests.Web.Candidate.SpecFlow.Hooks
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
