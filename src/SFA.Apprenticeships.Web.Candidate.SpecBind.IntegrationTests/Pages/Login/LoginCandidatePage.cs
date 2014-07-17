namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Login
{
    using System.Linq;
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    [PageNavigation("/login")]
    [PageAlias("LoginCandidatePage")]
    public class LoginCandidatePage
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

        // TODO: AG: triggerSignIn button id.
        [ElementLocator(Id = "triggerSignIn")]
        public IWebElement SignInButton { get; set; }

        [ElementLocator(Class = "validation-summary-errors")]
        public IElementList<IWebElement, ValidationSummaryItem> ValidationSummary { get; set; }

        public string ValidationSummaryCount { get { return ValidationSummary.Count().ToString(); } }

        [ElementLocator(CssSelector = "a")]
        public class ValidationSummaryItem : WebElement
        {
            public ValidationSummaryItem(ISearchContext parent)
                : base(parent)
            {
            }
        }
    }
}
