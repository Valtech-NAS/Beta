namespace SFA.Apprenticeships.Infrastructure.LocationLookup.IntegrationTests
{
    using System;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Elastic.Common.IoC;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LocationLookupProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
            });
        }

        [Test, Category("Integration")]
        public void ShouldMatchExactPlaceName()
        {
            // arrange
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
            const string term = "Cheylesmore";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(term, results.First().Name);
        }

        [Test, Category("Integration")]
        public void ShouldMatchFuzzyPlaceName()
        {
            // arrange
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
            const string term = "Chellsmore";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Cheylesmore", results.First().Name);
        }
    }
}
