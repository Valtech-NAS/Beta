namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Login
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/login")]
    [PageAlias("LoginCandidatePage")]
    public class LoginCandidatePage : BaseValidationPage
    {
        public LoginCandidatePage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "Password")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "sign-in-button")]
        public IWebElement SignInButton { get; set; }
    }
}