namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    using System.Linq;
    using ApprenticeshipSearch;
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/apprenticeshipsearch")]
    [PageAlias("ApprenticeshipSearchPage")]
    public class ApprenticeshipSearchPage : BaseValidationPage
    {
        private IWebElement _locationAutoComplete;

        public ApprenticeshipSearchPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "Keywords")]
        public IWebElement Keywords { get; set; }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "loc-within")]
        public IWebElement WithInDistance { get; set; }

        [ElementLocator(Id = "apprenticeship-level")]
        public IWebElement ApprenticeshipLevel { get; set; }

        [ElementLocator(Id = "search-button")]
        public IWebElement Search { get; set; }

        [ElementLocator(Class = "ui-autocomplete")]
        public IWebElement LocationAutoComplete
        {
            get { return _locationAutoComplete; }
            set { _locationAutoComplete = value; }
        }

        [ElementLocator(Class = "ui-autocomplete")]
        public IElementList<IWebElement, LocationAutoCompleteItem> LocationAutoCompletItems { get; set; }

        [ElementLocator(Id = "keywords-tab-control")]
        public IWebElement KeywordsTab { get; set; }

        [ElementLocator(Id = "categories-tab-control")]
        public IWebElement CategoriesTab { get; set; }

        [ElementLocator(Id = "categories")]
        public IWebElement Categories { get; set; }

        [ElementLocator(Id = "categories")]
        public IElementList<IWebElement, CategoryItem> CategoryItems { get; set; }

        public string CategoryItemsCount
        {
            get { return CategoryItems.Count().ToString(); }
        }

        [ElementLocator(Id = "browse-button")]
        public IWebElement Browse { get; set; }
    }
}