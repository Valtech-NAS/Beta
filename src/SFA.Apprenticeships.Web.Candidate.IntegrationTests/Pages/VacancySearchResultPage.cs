
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;
    using Common.IntegrationTests;

    public class VacancySearchResultPage : PageObject<VacancySearchResultPage>, ISfaPage
    {
        public const string PageTitle = "Apprenticeships";
        public const string Heading = "Search results";

        public VacancySearchResultPage(FluentTest test)
            : base(test)
        {
            Uri = new Uri(string.Format("{0}{1}", SiteConfig.WebRoot, @"vacancysearch/results"));
            At = () => I.Expect.Exists("html");
        }

        public void Verify()
        {
            I.Assert.Text(PageTitle).In(".global-header__title");
            I.Assert.Text(Heading).In(".heading-xlarge");
            I.Assert.Exists("#pagedList");
            I.Assert.Not.Exists("#pagedList .updating");
        }

        public void GoToPage()
        {
            Go();
            Verify();
        }
    }
}
