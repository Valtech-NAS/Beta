using OpenQA.Selenium;
using SpecBind.Pages;
using SpecBind.Selenium;

namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    public class SearchResultList : WebElement
    {
        public SearchResultList(ISearchContext parent)
            : base(parent)
        {

        }

        [ElementLocator(TagName = "LI")]
        public IWebElement HeaderItem { get; set; }
    }
}
