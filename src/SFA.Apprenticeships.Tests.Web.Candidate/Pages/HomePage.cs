namespace SFA.Apprenticeships.Tests.Web.Candidate.Pages
{
    using System;
    using FluentAutomation;
    using SFA.Apprenticeships.Tests.Web.Common;

    public class HomePage : PageObject<HomePage>
    {
        public HomePage(FluentTest test) : base(test)
        {
            Uri = new Uri(SiteConfig.WebRoot);
            At = () => I.Expect.Exists("html");
        }
    }
}
