namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.IoC;

    public class SetUp
    {
        [SetUp]
        public virtual void BeforeAllTests()
        {
            IoC.Initialize(new[] {"SFA.Apprenticeships.Common", "SFA.Apprenticeships.Common.Messaging.IntegrationTests"});
        }

        [TearDown]
        public virtual void AfterAllTests()
        {
        }
    }
}
