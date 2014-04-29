namespace SFA.Apprenticeships.Tests.Web.Candidate.Fluent
{
    using FluentAutomation;
    using NUnit.Framework;
    using SFA.Apprenticeships.Tests.Web.Common;

    [TestFixture]
    public class FluentTestShould : SFAFluentTest
    {
        [Test]
        public void ConnectAndValidateWebPageContent()
        {
            I.Open(WebRoot);
            I.Assert.Exists("#h1header");
        }
    }
}
