namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;
    using Common.IntegrationTests;

    public class VacancyDetailsPage : PageObject<VacancyDetailsPage>, IPageUnderTest
    {
        public const string PageTitle = "Find an Apprenticeship";
        public const string Heading = "Search results";

        public VacancyDetailsPage(FluentTest test)
            : base(test)
        {
            Uri = new Uri(string.Format("{0}{1}", SiteConfig.WebRoot, @"vacancysearch/details"));
            At = () => I.Expect.Exists("html");
        }

        public void Verify()
        {
            I.Assert.Exists(".global-header__title").Text(PageTitle);
            I.Assert.Exists(".heading-xlarge").Text(Heading);
            var results = I.Find("#pagedList")();
            I.WaitUntil(() => !results.Attributes.Get("class").Contains("updating"));
        }

        public void GoToPage()
        {
            Go();
            Verify();
        }
    }
}
