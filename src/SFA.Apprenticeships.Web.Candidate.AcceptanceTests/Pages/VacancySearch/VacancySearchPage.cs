namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/vacancysearch")]
    [PageAlias("VacancySearchPage")]
    public class VacancySearchPage : BaseValidationPage
    {
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

        //TODO: refactor to base class
        [ElementLocator(Id = "SuccessMessageText")]
        public IWebElement SuccessMessageText { get; set; }
    }
}