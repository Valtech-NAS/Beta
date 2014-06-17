namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Common
{
    using SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages;
    using TechTalk.SpecFlow;

    public abstract class CommonSteps
    {
        private IPageUnderTest _page;

        public IPageUnderTest Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                SetPage(value);
            }
        }

        public void SetPage(IPageUnderTest page)
        {
            ScenarioContext.Current.Remove("currentPageUnderTest");
            ScenarioContext.Current.Add("currentPageUnderTest", page);
        }

        public IPageUnderTest GetPage()
        {
            return ScenarioContext.Current.Get<IPageUnderTest>("currentPageUnderTest");
        }

        public void ClickButton(string buttonText)
        {
            Page.I.Click(string.Format(":button['{0}']", buttonText));
        }

        public void ClickLink(string linkText)
        {
            ClickLink("a.button-link", linkText);
        }

        public void ClickLink(string linkSelector, string linkText)
        {
            Page.I.Click(string.IsNullOrEmpty(linkText)
                ? linkSelector
                : string.Format("{0}['{1}']", linkSelector, linkText));
        }
    }
}
