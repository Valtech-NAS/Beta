namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.VacancySearch
{
    using System.Reflection;
    using global::SpecBind.BrowserSupport;
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    [PageNavigation("/vacancysearch")]
    [PageAlias("VacancySearchPage")]
    public class VacancySearchPage
    {
        private IWebDriver _driver;
        private readonly ISearchContext _context;

        public VacancySearchPage(IBrowser browser, ISearchContext context)
        {
            //These are here to show examples of gaining access to driver.
            _driver = Driver(browser);
            _context = context;
        }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "search-button")]
        public IWebElement Search { get; set; }

        private static IWebDriver Driver(IBrowser browser)
        {
            var field = typeof(SeleniumBrowser).GetField("driver", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            var value = field.GetValue(browser);
            return (value as System.Lazy<IWebDriver>).Value;
        }
    }
}