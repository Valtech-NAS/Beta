namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/vacancysearch")]
    [PageAlias("VacancySearchPage")]
    public class VacancySearchPage : BaseValidationPage
    {
        private IWebElement _locationAutoComplete;

        public VacancySearchPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "Keywords")]
        public IWebElement Keywords { get; set; }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "loc-within")]
        public IWebElement WithInDistance { get; set; }

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
    }
}