namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests
{
    using NUnit.Framework;

    [SetUpFixture]
    public class SetUp : Common.IntegrationTests.SetUp
    {
        [SetUp]
        public override void BeforeAllTests()
        {
            base.BeforeAllTests();
        }

        [TearDown]
        public override void AfterAllTests()
        {
            base.AfterAllTests();
        }
    }
}
