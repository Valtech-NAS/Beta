namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Tests
{
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Caching.IoC;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC;
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
                x.AddRegistry<CachingRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });
        }
    }
}
