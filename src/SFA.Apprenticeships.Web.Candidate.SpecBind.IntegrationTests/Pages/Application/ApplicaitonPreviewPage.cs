namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Application
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/application/preview/{VacancyId}")]
    [PageAlias("ApplicationPreviewPage")]
    public class ApplicationPreviewPage : BaseValidationPage
    {
        public ApplicationPreviewPage(ISearchContext context) : base(context)
        {
        }
    }
}