namespace SFA.Apprenticeships.Tests.Web.Employer.Fluent
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
            I.Assert.Text(text => text == "Employer Home page").In("#h1header");
        }
    }
}
