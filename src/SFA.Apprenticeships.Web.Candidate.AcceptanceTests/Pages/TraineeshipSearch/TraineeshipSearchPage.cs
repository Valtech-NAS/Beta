namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.TraineeshipSearch
{
    using ApprenticeshipSearch;
    using OpenQA.Selenium;
    using SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch;
    using SpecBind.Pages;

    [PageNavigation("/traineeshipsearch")]
    [PageAlias("TraineeshipSearchPage")]
    public class TraineeshipSearchPage : BaseValidationPage
    {
        private IWebElement _locationAutoComplete;

        public TraineeshipSearchPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

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