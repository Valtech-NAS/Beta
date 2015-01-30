namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(TagName = "li")]
    public class CategoryItem : WebElement
    {
        protected internal CategoryItem(ISearchContext searchContext) : base(searchContext)
        {
        }

        [ElementLocator(Name = "Category")]
        public IWebElement CategoryRadioButton { get; set; }

        [ElementLocator(TagName = "label")]
        public IWebElement CategoryLabel { get; set; }
    }
}