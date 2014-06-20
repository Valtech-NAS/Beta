namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Common
{
    using FluentAutomation;
    using Pages;
    using TechTalk.SpecFlow;

    public abstract class CommonSteps<T> : PageObject<T> where T : PageObject, ISfaPage
    {
        public CommonSteps(FluentTest test):base(test)
        {
        }

        public void ClickButton(string buttonText)
        {
            I.Click(string.Format(":button['{0}']", buttonText));
        }

        public void ClickLink(string linkText)
        {
            ClickLink("a.button-link", linkText);
        }

        public void ClickLink(string linkSelector, string linkText)
        {
            I.Click(string.IsNullOrEmpty(linkText)
                ? linkSelector
                : string.Format("{0}['{1}']", linkSelector, linkText));
        }
    }
}
