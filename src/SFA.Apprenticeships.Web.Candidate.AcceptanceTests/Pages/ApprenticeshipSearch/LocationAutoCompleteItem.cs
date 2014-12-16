namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(TagName = "li")]
    public class LocationAutoCompleteItem : WebElement
    {
        public LocationAutoCompleteItem(ISearchContext parent) : base(parent)
        {
        }
    }
}
