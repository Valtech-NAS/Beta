namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/vacancysearch/results")]
    [PageAlias("VacancySearchResultPage")]
    public class VacancySearchResultPage
    {
        private readonly ISearchContext _context;

        public VacancySearchResultPage(ISearchContext context)
        {
            _context = context;
        }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "search-button", Type = "submit", TagName = "button")]
        public IWebElement Search { get; set; }

        [ElementLocator(Id = "search-results")]
        public IElementList<IWebElement, SearchResultList> SearchResults { get; set; }
    }
}