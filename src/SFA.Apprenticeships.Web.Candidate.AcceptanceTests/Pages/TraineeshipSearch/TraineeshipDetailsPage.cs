namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.VacancySearch
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/traineeship/[0-9]+", UrlTemplate = "/traineeship/{VacancyId}")]
    [PageAlias("TraineeshipDetailsPage")]
    public class TraineeshipDetailsPage
    {
        [ElementLocator(Id = "lnk-return-search-results")]
        public IWebElement ReturnToSearchResultsLink { get; set; }

        [ElementLocator(Id = "lnk-find-traineeship")]
        public IWebElement FindApprenticeshipLink { get; set; }

        [ElementLocator(Id = "apply-button")]
        public IWebElement ApplyButton { get; set; }

        [ElementLocator(Id="external-employer-website")]
        public IWebElement ApplyExternalLink { get; set; }

        [ElementLocator(TagName = "h1")]
        public IWebElement ApprenticeshipNoLongerAvailableHeading { get; set; }
    }
}