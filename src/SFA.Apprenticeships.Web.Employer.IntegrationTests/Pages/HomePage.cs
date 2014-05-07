namespace SFA.Apprenticeships.Web.Employer.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;
    using SFA.Apprenticeships.Web.Common.IntegrationTests;

    public class HomePage : PageObject<HomePage>
    {
        public HomePage(FluentTest test) : base(test)
        {
            Uri = new Uri(SiteConfig.WebRoot);
            At = () => I.Expect.Exists("html");
        }
    }
}
