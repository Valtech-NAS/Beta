namespace SFA.Apprenticeships.Tests.Web.Candidate.Fluent
{
    using FluentAutomation;
    using NUnit.Framework;
    using SFA.Apprenticeships.Tests.Web.Common;

    [TestFixture]
    public class FluentTestShould : FluentTest
    {
        [Test]
        public void ConnectAndValidateWebPageContent()
        {
            I.Open(SiteConfig.WebRoot);
            I.Assert.Exists("#h1header");
        }
    }
}
