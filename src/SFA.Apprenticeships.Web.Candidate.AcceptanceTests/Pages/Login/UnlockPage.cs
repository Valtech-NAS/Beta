namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Login
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/login/unlock")]
    [PageAlias("UnlockPage")]
    public class UnlockPage : BaseValidationPage
    {
        public UnlockPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "EmailAddressText")]
        public IWebElement EmailAddressText { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "AccountUnlockCode")]
        public IWebElement AccountUnlockCode { get; set; }

        [ElementLocator(Id = "verify-code-button")]
        public IWebElement VerifyCodeButton { get; set; }

        [ElementLocator(Id = "ResendAccountUnlockCodeLink")]
        public IWebElement ResendAccountUnlockCodeLink { get; set; }

        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement ResentCodeText { get; set; }
    }
}
