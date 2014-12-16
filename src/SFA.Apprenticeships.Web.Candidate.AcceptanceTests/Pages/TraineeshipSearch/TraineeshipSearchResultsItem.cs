namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.TraineeshipSearch
{
    using System.Linq;
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(Class = "search-results__item")]
    public class TraineeshipSearchResultsItem : WebElement
    {
        protected internal TraineeshipSearchResultsItem(ISearchContext searchContext)
            : base(searchContext)
        {
        }

        [ElementLocator(Class = "list-text")]
        public IElementList<IWebElement, PropertyListItem> PropertyList { get; set; }

        public string DistanceDisplayed
        {
            get { return PropertyList.Any(i => i.Text.StartsWith("Distance:")).ToString(); }
        }

        public string ClosingDateDisplayed
        {
            get { return PropertyList.Any(i => i.Text.StartsWith("Closing date:")).ToString(); }
        }
    }

    [ElementLocator(TagName = "li")]
    public class PropertyListItem : WebElement
    {
        protected internal PropertyListItem(ISearchContext searchContext) : base(searchContext)
        {
        }
    }
}