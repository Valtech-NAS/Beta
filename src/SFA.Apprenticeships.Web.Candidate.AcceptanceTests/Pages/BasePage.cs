namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

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

        [ElementLocator(Id = "loginLink")]
        public IWebElement SignInLink { get; set; }

        [ElementLocator(Id = "bannerUserName")]
        public IWebElement BannerUserName { get; set; }

        [ElementLocator(Id = "signout-link")]
        public IWebElement SignOutLink { get; set; }

        [ElementLocator(Id = "mysettings-link")]
        public IWebElement MySettingsLink { get; set; }

        [ElementLocator(Id = "myapplications-link")]
        public IWebElement MyApplicationsLink { get; set; }

        [ElementLocator(Id = "InfoMessageText")]
        public IWebElement InfoMessageText { get; set; }

        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageText { get; set; }

        [ElementLocator(Id = "WarningMessageText")]
        public IWebElement WarningMessageText { get; set; }

        [ElementLocator(Id = "ErrorMessageText")]
        public IWebElement ErrorMessageText { get; set; }
    }
}