namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages
{
    using OpenQA.Selenium;

    public class BasePage
    {
        protected readonly ISearchContext Context;
        protected readonly IWebDriver Driver;

        public BasePage(ISearchContext context)
        {
            Context = context;
        }

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public BasePage(IWebDriver driver, ISearchContext context)
        {
            Context = context;
            Driver = driver;
        }
    }
}