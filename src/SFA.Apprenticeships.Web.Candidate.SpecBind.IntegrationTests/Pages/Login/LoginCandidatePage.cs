namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Login
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/login")]
    [PageAlias("LoginCandidatePage")]
    public class LoginCandidatePage : BaseValidationPage
    {
        private readonly ISearchContext _context;

        public LoginCandidatePage(ISearchContext context)
        {
            _context = context;
        }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "Password")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "signInButton")]
        public IWebElement SignInButton { get; set; }
    }
}