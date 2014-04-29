namespace SFA.Apprenticeships.Tests.Services.Common
{
    using NUnit.Framework;

    [TestFixture]
    public class TestShould
    {
        [Test]
        public void Pass()
        {
            Assert.True(true);
        }

        [Test]
        public void Fail()
        {
            Assert.True(false);
        }
    }
}
