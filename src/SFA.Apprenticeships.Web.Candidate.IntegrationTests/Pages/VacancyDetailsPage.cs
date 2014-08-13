namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Pages
{
    using System;
    using FluentAutomation;

    public class VacancyDetailsPage : PageObject<VacancyDetailsPage>, ISfaPage
    {
        public const string PageTitle = "Apprenticeships";
        public const string Heading = "Search results";

        public VacancyDetailsPage(FluentTest test)
            : base(test)
        {
            //Uri = new Uri(string.Format("{0}{1}", SiteConfig.WebRoot, @"vacancysearch/details"));
            At = () => I.Expect.Exists("html");
        }

        public void Verify()
        {
            DateTime closingDate;
            I.Assert.Text(PageTitle).In(".global-header__title");
            I.Assert.Text(text => text.Length > 0).In(".heading-xlarge");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-description");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-subtitle-employer-name");
            I.Assert.Text(text => DateTime.TryParse(text, out closingDate)).In("#vacancy-closing-date");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-full-descrpition");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-wage");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-working-week");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-expected-duration");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-distance");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-reference-id");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-skills-required");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-qualifications-required");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-employer-description");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-employer-name");
            //I.Assert.Text(text => text.Length > 0).In("#vacancy-employer-website");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-training-to-be-provided");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-framework");
            I.Assert.Text(text => text.Length > 0).In("#vacancy-provider-name");
            //I.Assert.Text(text => text.Length > 0).In("#vacancy-provider-contact");
        }
    }
}
