namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    using System.Linq;
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    public class BaseValidationPage
    {
        public BaseValidationPage(ISearchContext context)
        {
        }
        public BaseValidationPage()
        {
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