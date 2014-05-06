namespace SFA.Apprenticeships.Tests.Web.Common
{
    using FluentAutomation;
    using NUnit.Framework;

    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public virtual void BeforeAllTests()
        {
            SeleniumWebDriver.Bootstrap(SeleniumWebDriver.Browser.Firefox);
            FluentSession.EnableStickySession();
        }

        [TearDown]
        public virtual void AfterAllTests()
        {
        }
    }
}
