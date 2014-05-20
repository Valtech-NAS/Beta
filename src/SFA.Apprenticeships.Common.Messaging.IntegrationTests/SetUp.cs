namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using NUnit.Framework;
    
    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public virtual void BeforeAllTests()
        {
            Common.IoC.IoC.Initialize();
        }

        [TearDown]
        public virtual void AfterAllTests()
        {
        }
    }
}
