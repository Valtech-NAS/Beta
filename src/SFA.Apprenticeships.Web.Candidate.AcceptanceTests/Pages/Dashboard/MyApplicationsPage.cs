namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Dashboard
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/myapplications")]
    [PageAlias("MyApplicationsPage")]
    public class MyApplicationsPage : BaseValidationPage
    {
        public MyApplicationsPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "draft-applications-count")]
        public IWebElement DraftApplicationsCount { get; set; }

        [ElementLocator(Id = "submitted-applications-count")]
        public IWebElement SubmittedApplicationsCount { get; set; }

        [ElementLocator(Id = "resume-link")]
        public IWebElement ResumeLink { get; set; }

        [ElementLocator(Id = "delete-link")]
        public IWebElement DeleteLink { get; set; }

        [ElementLocator(Id = "empty-application-history-text")]
        public IWebElement EmptyApplicationHistoryText { get; set; }
    }
}