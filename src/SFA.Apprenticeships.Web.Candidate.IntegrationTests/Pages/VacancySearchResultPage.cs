
namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;
    using SFA.Apprenticeships.Web.Common.IntegrationTests;

    public class VacancySearchResultPage : PageObject<VacancySearchResultPage>, IPageUnderTest
    {
        public const string PageTitle = "Find an Apprenticeship";
        public const string Heading = "Search results";

        public VacancySearchResultPage(FluentTest test)
            : base(test)
        {
            Uri = new Uri(string.Format("{0}{1}", SiteConfig.WebRoot, @"vacancysearch/results"));
            At = () => I.Expect.Exists("html");
        }

        public void Verify()
        {
            I.Assert.Exists(".global-header__title").Text(PageTitle);
            I.Assert.Exists(".heading-xlarge").Text(Heading);
        }

        public void GoToPage()
        {
            Go();
            Verify();
        }
    }
}
