namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using Caching.Memory.IoC;
    using Common.IoC;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public void SetUpTests()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<GatewayWebServicesRegistry>();
            });
        }
    }
}