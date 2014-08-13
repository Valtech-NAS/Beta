
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;
    using SFA.Apprenticeships.Web.Common.IntegrationTests;

    public class VacancySearchIndexPage : PageObject<VacancySearchIndexPage>, ISfaPage
    {
        public const string PageTitle = "Apprenticeships";
        public const string Heading = "Find an Apprenticeship";

        public VacancySearchIndexPage(FluentTest test)
            : base(test)
        {
            Uri = new Uri(string.Format("{0}{1}", SiteConfig.WebRoot, @"vacancysearch/index"));
            Go();
            At = () => I.Expect.Exists("html");
        }

        public void Verify()
        {
            I.Assert.Text(PageTitle).In(".global-header__title");
            I.Assert.Text(Heading).In(".heading-xlarge");
        }
    }
}
