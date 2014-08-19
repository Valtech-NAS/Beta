namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/application/preview/[0-9]+")]
    [PageAlias("ApplicationPreviewPage")]
    public class ApplicationPreviewPage : BaseValidationPage
    {
        public ApplicationPreviewPage(ISearchContext context) : base(context)
        {
        }
    }
}