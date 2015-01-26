namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System.Linq;
    using Application.ReferenceData;
    using Caching.Memory.IoC;
    using Common.IoC;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ReferenceDataProviderTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });
            _referenceDataProvider = ObjectFactory.GetInstance<IReferenceDataProvider>();
#pragma warning restore 0618            
        }

        [Test, Category("Integration")]
        public void ReturnsCategoryDataFromFrameworksService()
        {
            var categories = _referenceDataProvider.GetCategories();
            categories.Count().Should().BeGreaterThan(0);

            foreach (Category category in categories)
            {
                category.SubCategories.Count().Should().BeGreaterThan(0);
            }
        }

    }
}
