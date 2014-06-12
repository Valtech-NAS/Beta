
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;
    using SFA.Apprenticeships.Web.Common.IntegrationTests;

    public class VacancySearchIndexPage : PageObject<VacancySearchIndexPage>, IPageUnderTest
    {
        public const string PageTitle = "Find an Apprenticeship";

        public VacancySearchIndexPage(FluentTest test)
            : base(test)
        {
            Uri = new Uri(string.Format("{0}{1}", SiteConfig.WebRoot, @"vacancysearch/index"));
            At = () => I.Expect.Exists("html");
        }

        public void Verify()
        {
            I.Assert.Exists(".global-header__title beta");
            I.Assert.Exists(".global-header__title beta").Text(PageTitle);
        }
    }
}
