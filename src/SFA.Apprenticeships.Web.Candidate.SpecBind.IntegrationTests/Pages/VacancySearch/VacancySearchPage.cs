namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.VacancySearch
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

        [ElementLocator(Id = "search-button")]
        public IWebElement Search { get; set; }
    }
}