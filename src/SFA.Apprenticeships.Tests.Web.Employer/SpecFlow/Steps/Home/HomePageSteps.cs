namespace SFA.Apprenticeships.Tests.Web.Employer.SpecFlow.Steps.Home
{
    using SFA.Apprenticeships.Tests.Web.Employer.Pages;
    using Specflow.FluentAutomation.Ext;
    using TechTalk.SpecFlow;

    [Binding]
    public class HomePageSteps
    {
        [Given(@"I navigated to the homepage")]
        public void GivenINavigatedToTheHomepage()
        {
            Pages.Get<HomePage>().Go();
        }

        [Then(@"the screen has the title of '(.*)'")]
        public void ThenTheScreenHasTheTitleOf(string pageTitle)
        {
            Pages.Get<HomePage>().I.Assert.Exists("#h1header");
            Pages.Get<HomePage>().I.Assert.Exists("#h1header").Text(pageTitle);
        }

    }
}
