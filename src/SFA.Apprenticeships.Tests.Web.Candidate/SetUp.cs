namespace SFA.Apprenticeships.Tests.Web.Candidate
{
    using NUnit.Framework;

    [SetUpFixture]
    public class SetUp : Common.SetUp
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
