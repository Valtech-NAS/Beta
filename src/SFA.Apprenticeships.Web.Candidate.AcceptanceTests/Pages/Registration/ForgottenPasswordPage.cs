namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Registration
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/register/forgottenpassword")]
    [PageAlias("ForgottenPasswordPage")]
    public class ForgottenPasswordPage : BaseValidationPage
    {
        public ForgottenPasswordPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "forgottenpassword-button")]
        public IWebElement SendCodeButton { get; set; }

        [ElementLocator(Text = "enter it")]
        public IWebElement UnlockAccountLink { get; set; }
    }
}