namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.TraineeshipApplication
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/traineeship/whatnext/[0-9]+")]
    [PageAlias("TraineeshipWhatsNextPage")]
    public class TraineeshipWhatsNextPage : BasePage
    {
        public TraineeshipWhatsNextPage(ISearchContext context) : base(context)
        {
        }
    }
}