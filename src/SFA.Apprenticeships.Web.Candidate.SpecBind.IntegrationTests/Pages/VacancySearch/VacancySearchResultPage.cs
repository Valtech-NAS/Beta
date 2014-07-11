namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.VacancySearch
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
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

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, SearchResultLi> SearchResults { get; set; }
    }

    [ElementLocator(Class = "search-results__item")]
    public class SearchResultLi : WebElement
    {
        public SearchResultLi(ISearchContext parent) : base(parent)
        {
        }

        [ElementLocator(Class = "vacancy-title-link")]
        public IWebElement Title { get; set; }

        [ElementLocator(Class = "subtitle")]
        public IWebElement Subtitle { get; set; }

        [ElementLocator(Class = "search-shortdesc")]
        public IWebElement ShortDescription { get; set; }
    }
}