namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipApplication
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/apprenticeship/whatnext/[0-9]+")]
    [PageAlias("ApprenticeshipApplicationCompletePage")]
    public class ApprenticeshipApplicationCompletePage : BasePage
    {
        public ApprenticeshipApplicationCompletePage(ISearchContext context) : base(context)
        {
        }
    }
}