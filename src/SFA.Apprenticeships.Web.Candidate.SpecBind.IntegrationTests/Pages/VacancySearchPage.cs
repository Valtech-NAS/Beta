namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/vacancysearch")]
    [PageAlias("VacancySearchPage")]
    public class VacancySearchPage
    {
        private readonly ISearchContext _context;

        public VacancySearchPage(ISearchContext context)
        {
            _context = context;
        }

        [ElementLocator(Id = "Location")]
        public IWebElement Location { get; set; }

        [ElementLocator(Id = "search-button")]
        public IWebElement Search { get; set; }
    }
}