namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.TraineeshipSearch
{
    using System;
    using System.Linq;
    using OpenQA.Selenium;
    using SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [PageNavigation("/traineeships")]
    [PageAlias("TraineeshipSearchResultPage")]
    public class TraineeshipSearchResultPage : BaseValidationPage
    {
        private IWebElement _locationAutoComplete;

        public TraineeshipSearchResultPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        public string ClearLocation
        {
            get
            {
                Location.Clear();
                return "True";
            }
        }

        [ElementLocator(Id = "loc-within")]
        public IWebElement WithInDistance { get; set; }

        [ElementLocator(Id = "search-button")]
        public IWebElement Search { get; set; }

        [ElementLocator(Id = "sort-results")]
        public IWebElement SortOrderingDropDown { get; set; }

        [ElementLocator(Id = "sort-results")]
        public IElementList<IWebElement, SortOrderingDropDownItem> SortOrderingDropDownItems { get; set; }

        public string SortOrderingDropDownItemsText
        {
            get { return string.Join(",", SortOrderingDropDownItems.Select(i => i.Text)); }
        }

        public string SortOrderingDropDownItemsCount
        {
            get { return SortOrderingDropDownItems.Count().ToString(); }
        }

        [ElementLocator(Id = "results-per-page")]
        public IWebElement ResultsPerPageDropDown { get; set; }

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, TraineeshipSearchResultsItem> SearchResults { get; set; }

        [ElementLocator(Class = "next")]
        public IWebElement NextPage { get; set; }

        [ElementLocator(Class = "previous")]
        public IWebElement PreviousPage { get; set; }

        [ElementLocator(Id = "search-no-results-title")]
        public IWebElement NoResultsTitle { get; set; }

        [ElementLocator(Id = "nationwideLocationTypeLink")]
        public IWebElement NationwideLocationTypeLink { get; set; }

        [ElementLocator(Id = "localLocationTypeLink")]
        public IWebElement LocalLocationTypeLink { get; set; }

        [ElementLocator(Class = "ui-autocomplete")]
        public IWebElement LocationAutoComplete
        {
            get { return _locationAutoComplete; }
            set { _locationAutoComplete = value; }
        }

        [ElementLocator(Class = "ui-autocomplete")]
        public IElementList<IWebElement, LocationAutoCompleteItem> LocationAutoCompletItems { get; set; }

        [ElementLocator(Class = "search-results")]
        public IElementList<IWebElement, TraineeshipSearchResultItem> SearchResultItems { get; set; }

        public TraineeshipSearchResultItem FirstResultItem
        {
            get { return SearchResultItems.FirstOrDefault(); }
        }

        public TraineeshipSearchResultItem SecondResultItem
        {
            get
            {
                return SearchResultItems != null && SearchResultItems.Count() >= 2 ? SearchResultItems.Skip(1).First() : null;
            }
        }

        public string SearchResultItemsCount
        {
            get { return SearchResultItems.Count().ToString(); }
        }

        [ElementLocator(Id = "location-suggestions")]
        public IWebElement LocationSuggestionsContainer { get; set; }

        [ElementLocator(Id = "location-suggestions")]
        public IElementList<IWebElement, LocationSuggestion> LocationSuggestions { get; set; }

        public string LocationSuggestionsCount
        {
            get { return LocationSuggestions.Count().ToString(); }
        }

        public string ResultsAreInDistanceOrder
        {
            get
            {
                bool result = true;
                TraineeshipSearchResultItem previousItem = null;
                for (int i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (i > 0)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currentDistance = double.Parse(currentItem.Distance.Text);
                        var previousDistance = double.Parse(previousItem.Distance.Text);
                        result = result & currentDistance >= previousDistance;

                    }
                    previousItem = SearchResultItems.ElementAt(i);
                }

                return result.ToString();
            }
        }

        public string ResultsAreInClosingDateOrder
        {
            get
            {
                bool result = true;
                TraineeshipSearchResultItem previousItem = null;
                for (int i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (i > 0)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currrentClosingDate = DateTime.Parse(currentItem.ClosingDate.GetAttribute("data-date"));
                        var previousClosingDate = DateTime.Parse(previousItem.ClosingDate.GetAttribute("data-date"));
                        result = result & currrentClosingDate >= previousClosingDate;
                    }
                    previousItem = SearchResultItems.ElementAt(i);
                }

                return result.ToString();
            }
        }

        public string ResultsAreInBestMatchScoreOrder
        {
            get
            {
                bool result = true;
                TraineeshipSearchResultItem previousItem = null;
                for (var i = 0; i < SearchResultItems.Count(); i++)
                {
                    if (i > 0)
                    {
                        var currentItem = SearchResultItems.ElementAt(i);
                        var currentClosing = double.Parse(currentItem.BestMatchScore.Text);
                        var previousClosing = double.Parse(previousItem.BestMatchScore.Text);
                        result = result & previousClosing >= currentClosing;
                    }
                    previousItem = SearchResultItems.ElementAt(i);

                    if (previousItem.BestMatchScore == null)
                    {
                        return false.ToString();
                    }
                }

                return result.ToString();
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

        public IWebElement FirstVacancyId
        {
            get
            {
                var item = SearchResultItems.First();
                return item;
            }
        }
    }

    [ElementLocator(Class = "search-results__item")]
    public class TraineeshipSearchResultItem : WebElement
    {
        public TraineeshipSearchResultItem(ISearchContext parent)
            : base(parent)
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

        [ElementLocator(Class = "best-match-score")]
        public IWebElement BestMatchScore { get; set; }

        public override string Text
        {
            get { return VacancyId; }
        }
    }
}