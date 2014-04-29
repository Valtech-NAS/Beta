namespace SFA.Apprenticeships.Tests.Web.Candidate.Pages
{
    using System;
    using System.Configuration;
    using FluentAutomation;

    public class HomePage : PageObject<HomePage>
    {
        public HomePage(FluentTest test) : base(test)
        {
            Uri = new Uri(ConfigurationManager.AppSettings["WebRoot"]);
            At = () => I.Expect.Exists("html");
        }
    }
}
