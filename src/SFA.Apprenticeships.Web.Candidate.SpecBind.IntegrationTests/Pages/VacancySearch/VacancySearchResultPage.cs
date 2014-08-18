namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.VacancySearch
{
    using System;
    using System.Linq;
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
        
        [ElementLocator(Id = "Keywords")]
        public IWebElement Keywords { get; set; }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "search-button", Type = "submit", TagName = "button")]
        public IWebElement Search { get; set; }

        [ElementLocator(Class = "search-results")]
        public IWebElement SearchResults { get; set; }

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, SearchResultItem> SearchResultItems { get; set; }

        public string SearchResultItemsCount
        {
            get { return SearchResultItems.Count().ToString(); }
        }

        public IWebElement FirstVacancyLink
        {
            get
            {
                var item = SearchResultItems.First();
                return item.VacancyLink;
            }
        }

        public string FirstVacancyId
        {
            get
            {
                var item = SearchResultItems.First();
                return item.VacancyId;
            }
        }
    }

    [ElementLocator(Class = "search-results__item")]
    public class SearchResultItem : WebElement
    {
        public SearchResultItem(ISearchContext parent) : base(parent)
        {
        }

        public string VacancyId
        {
            get { return VacancyLink.GetAttribute("data-vacancy-id"); }
        }

        [ElementLocator(Class = "vacancy-link")]
        public IWebElement VacancyLink { get; set; }

        [ElementLocator(Class = "vacancy-title-link")]
        public IWebElement Title { get; set; }

        [ElementLocator(Class = "subtitle")]
        public IWebElement Subtitle { get; set; }

        [ElementLocator(Class = "search-shortdesc")]
        public IWebElement ShortDescription { get; set; }
    }
}