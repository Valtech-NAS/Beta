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

        [ElementLocator(Id = "successful-applications-count")]
        public IWebElement SuccessfulApplicationsCount { get; set; }

        [ElementLocator(Id = "unsuccessful-applications-count")]
        public IWebElement UnsuccessfulApplicationsCount { get; set; }

        [ElementLocator(Id = "resume-link")]
        public IWebElement ResumeLink { get; set; }

        [ElementLocator(Class = "delete-draft")]
        public IWebElement DeleteDraftLink { get; set; }

        [ElementLocator(Class = "archive-successful")]
        public IWebElement ArchiveSuccessfulLink { get; set; }

        [ElementLocator(Class = "archive-unsuccessful")]
        public IWebElement ArchiveUnsuccessfulLink { get; set; }

        [ElementLocator(Id = "empty-application-history-text")]
        public IWebElement EmptyApplicationHistoryText { get; set; }
    }
}