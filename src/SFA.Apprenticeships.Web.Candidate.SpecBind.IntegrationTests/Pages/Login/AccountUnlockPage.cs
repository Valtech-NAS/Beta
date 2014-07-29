namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Login
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/login")]
    [PageAlias("AccountUnlockPage")]
    public class AccountUnlockPage : BaseValidationPage
    {
        public AccountUnlockPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "AccountUnlockCode")]
        public IWebElement AccountUnlockCode { get; set; }

        [ElementLocator(Id = "verify-code-button")]
        public IWebElement VerifyCodeButton { get; set; }
    }
}
