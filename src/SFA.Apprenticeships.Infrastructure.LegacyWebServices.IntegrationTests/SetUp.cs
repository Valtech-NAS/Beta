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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });
#pragma warning restore 0618
        }
    }
}