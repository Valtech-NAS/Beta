namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Registration
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/register/activation")]
    [PageAlias("ActivationPage")]
    public class ActivationPage : BaseValidationPage
    {
        public ActivationPage(ISearchContext context)
            : base(context)
        {
        }

        [ElementLocator(Id = "ActivationCode")]
        public IWebElement ActivationCode { get; set; }

        [ElementLocator(Id = "activate-button")]
        public IWebElement ActivateButton { get; set; }

        [ElementLocator(Class = "email-address")]
        public IWebElement EmailAddress { get; set; }
        
    }
}