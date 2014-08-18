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
    }
}