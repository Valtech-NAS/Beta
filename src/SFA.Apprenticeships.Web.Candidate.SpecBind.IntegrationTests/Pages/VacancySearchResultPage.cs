

using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using OpenQA.Selenium;
using SpecBind.Pages;

namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    [PageNavigation("/vacancysearch/results")]
    [PageAlias("VacancySearchResultPage")]
    public class VacancySearchResultPage
    {
        public VacancySearchResultPage(ISearchContext context)
        {

        }

        [ElementLocator(Id = "Location")]
        public HtmlEdit FindByLocation { get; set; }

        [ElementLocator(Id = "SearchButton", Type = "submit")]
        public HtmlButton Search { get; set; }

        [ElementLocator(Id = "search-results")]
        public IElementList<IWebElement, SearchResultList> SearchResults { get; set; }
    }
}
