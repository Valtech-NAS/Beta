using OpenQA.Selenium;
using SpecBind.Pages;
using SpecBind.Selenium;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    [ElementLocator(TagName = "li")]
    public class LocationSuggestion : WebElement
    {
        protected internal LocationSuggestion(ISearchContext searchContext) : base(searchContext)
        {
        }
    }
}