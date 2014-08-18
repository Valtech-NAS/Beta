namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    [PageNavigation("/vacancysearch/results")]
    [PageAlias("VacancySearchResultPage")]
    public class VacancySearchResultPage : BaseValidationPage
    {
        public VacancySearchResultPage(ISearchContext context) : base(context)
        {
        }
        
        [ElementLocator(Id = "Keywords")]
        public IWebElement Keywords { get; set; }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "search-button", Type = "submit", TagName = "button")]
        public IWebElement Search { get; set; }

        [ElementLocator(Id = "sort-results")]
        public IWebElement SortOrderingDropDown { get; set; }

        [ElementLocator(Class = "search-results")]
        public IWebElement SearchResults { get; set; }

        [ElementLocator(Class = "next")]
        public IWebElement NextPage { get; set; }

        [ElementLocator(Class = "previous")]
        public IWebElement PreviousPage { get; set; }

        [ElementLocator(Id = "search-no-results-title")]
        public IWebElement NoResultsTitle { get; set; }

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, SearchResultItem> SearchResultItems { get; set; }

        public string SearchResultItemsCount
        {
            get { return SearchResultItems.Count().ToString(); }
        }

        public string ResultsAreInDistanceOrder
        {
            get
            {
                SearchResultItem previousItem = null;
                for (int i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (i > 0)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currentDistance = double.Parse(currentItem.Distance.Text);
                        var previousDistance = double.Parse(previousItem.Distance.Text);
                        currentDistance.Should().BeGreaterOrEqualTo(previousDistance);
                    }
                    previousItem = SearchResultItems.ElementAt(i);
                }

                return "True";
            }
        }

        public string ResultsAreInClosingDateOrder
        {
            get
            {
                SearchResultItem previousItem = null;
                for (int i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (i > 0)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currentClosing = DateTime.Parse(currentItem.ClosingDate.Text);
                        var previousClosing = DateTime.Parse(previousItem.ClosingDate.Text);
                        currentClosing.Should().BeOnOrAfter(previousClosing);
                    }
                    previousItem = SearchResultItems.ElementAt(i);
                }

                return "True";
            }
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

        [ElementLocator(Class = "distance-value")]
        public IWebElement Distance { get; set; }

        [ElementLocator(Class = "closing-date-value")]
        public IWebElement ClosingDate { get; set; }
    }
}