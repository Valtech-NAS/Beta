namespace SFA.Apprenticeships.Tests.Web.Candidate.SpecFlow.Steps.Home
{
    using System;
    using System.CodeDom;
    using System.Configuration;
    using SFA.Apprenticeships.Tests.Web.Candidate.Pages;
    using TechTalk.SpecFlow;
    using Specflow.FluentAutomation.Ext;

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
