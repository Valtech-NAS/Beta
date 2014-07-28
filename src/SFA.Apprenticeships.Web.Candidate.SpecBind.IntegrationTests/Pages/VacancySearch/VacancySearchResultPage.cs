﻿namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.VacancySearch
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

        public Tuple<IWebElement, SearchResultItem> FirstSearchResultItem()
        {
            var item = SearchResultItems.First();

            return new Tuple<IWebElement, SearchResultItem>(item.WrappedElement, item);
        }
    }

    [ElementLocator(Class = "search-results__item")]
    public class SearchResultItem : WebElement
    {
        public SearchResultItem(ISearchContext parent) : base(parent)
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