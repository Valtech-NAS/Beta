namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.Fluent
{
    using FluentAutomation;
    using NUnit.Framework;
    using SFA.Apprenticeships.Web.Common.IntegrationTests;

    [TestFixture]
    public class FluentTestShould : FluentTest
    {
        [Test]
        public void ConnectAndValidateWebPageContent()
        {
            I.Open(SiteConfig.WebRoot);
            I.Assert.Exists("#h1header");
            I.Assert.Text(text => text == "Find an apprenticeship").In("#h1header");
        }
    }
}
