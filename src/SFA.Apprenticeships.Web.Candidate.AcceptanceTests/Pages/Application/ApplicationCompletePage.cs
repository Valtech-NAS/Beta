namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/apprenticeship/whatnext/[0-9]+")]
    [PageAlias("ApplicationCompletePage")]
    public class ApplicationCompletePage : BasePage
    {
        public ApplicationCompletePage(ISearchContext context) : base(context)
        {
        }
    }
}