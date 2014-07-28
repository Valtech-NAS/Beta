namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    using System.Linq;
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    public class BaseValidationPage
    {
        //TODO: Maybe push this and webdriver into a further BasePage class that we can use for all pages.
        protected readonly ISearchContext _context;

        public BaseValidationPage(ISearchContext context)
        {
            _context = context;
        }

        [ElementLocator(Class = "validation-summary-errors")]
        public IWebElement ValidationSummary { get; set; }

        [ElementLocator(Class = "validation-summary-errors")]
        public IElementList<IWebElement, ValidationSummaryItem> ValidationSummaryItems { get; set; }

        public string ValidationSummaryCount
        {
            get { return ValidationSummaryItems.Count().ToString(); }
        }

        [ElementLocator(TagName = "a")]
        public class ValidationSummaryItem : WebElement
        {
            public ValidationSummaryItem(ISearchContext parent)
                : base(parent)
            {
            }

            public string Href
            {
                get { return GetAttribute("href").Substring(GetAttribute("href").IndexOf("#")); }
            }
        }
    }
}