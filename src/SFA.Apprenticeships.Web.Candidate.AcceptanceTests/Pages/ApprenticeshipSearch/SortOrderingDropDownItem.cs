using OpenQA.Selenium;
using SpecBind.Pages;
using SpecBind.Selenium;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    [ElementLocator(TagName = "option")]
    public class SortOrderingDropDownItem : WebElement
    {
        protected internal SortOrderingDropDownItem(ISearchContext searchContext) : base(searchContext)
        {
        }
    }
}