namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Hooks
{
    using BoDi;
    using global::SpecBind.BrowserSupport;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class BeforeAllScenarios
    {
        private readonly IBrowser _browser;
        private readonly IObjectContainer _objectContainer;
        private readonly IWebDriver _driver;

        public BeforeAllScenarios()
        {
        }

        [BeforeScenario]
        public void Before()
        {
            //_driver.Manage().Cookies.DeleteAllCookies();
        }

        [AfterScenario]
        public void After()
        {
        }
    }
}
