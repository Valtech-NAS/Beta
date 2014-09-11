namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Registration
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/register/resetpassword")]
    [PageAlias("ResetPasswordPage")]
    public class ResetPasswordPage : BaseValidationPage
    {
        public ResetPasswordPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "PasswordResetCode")]
        public IWebElement PasswordResetCode { get; set; }

        [ElementLocator(Id = "Password")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "ConfirmPassword")]
        public IWebElement ConfirmPassword { get; set; }

        [ElementLocator(Id = "reset-password-button")]
        public IWebElement ResetPasswordButton { get; set; }
    }
}