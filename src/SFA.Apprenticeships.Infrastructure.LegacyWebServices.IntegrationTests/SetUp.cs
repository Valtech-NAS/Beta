namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using NUnit.Framework;
    using Caching.Memory.IoC;
    using Common.IoC;
    using IoC;
    using StructureMap;

    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public void SetUpTests()
        {
            var test = new LegacyWebServicesRegistry();
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });
        }
    }
}
