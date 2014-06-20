namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Hooks
{
    using Fluent;
    using FluentAutomation;
    using FluentAutomation.Interfaces;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using TechTalk.SpecFlow;

    [Binding]
    public class BeforeNoJavaScriptScenarios
    {

        [BeforeScenario("browser:disableJavaScript")]
        public static void Before()
        {
            //var p = new FirefoxProfile();
            //p.SetPreference("javascript.enabled", false);
            //var driver = new FirefoxDriver(p);

            //FluentSettings.Current.ContainerRegistration = container =>
            //{
            //    container.Register<ICommandProvider, CommandProvider>();
            //    container.Register<IAssertProvider, AssertProvider>();
            //    container.Register<IFileStoreProvider, LocalFileStoreProvider>();
            //    container.Register<IWebDriver>((cContainer, overloads) =>
            //    {
            //        return driver;
            //    });
            //};
        }

        [AfterScenario("browser:disableJavaScript")]
        public static void After()
        {
            
        }

    }
}
