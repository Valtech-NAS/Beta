

using OpenQA.Selenium;
using SpecBind.Pages;

namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    [PageNavigation("/vacancysearch")]
    [PageAlias("VacancySearchPage")]
    public class VacancySearchPage
    {
        public VacancySearchPage(ISearchContext context)
        {

        }

        [ElementLocator(Id = "Location")]
        public IWebElement FindByLocation { get; set; }

        [ElementLocator(Id = "SearchButton", Type = "submit")]
        public IWebElement Search { get; set; }
    }
}
