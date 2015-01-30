namespace SFA.Apprenticeships.Infrastructure.LocationLookup.IntegrationTests
{
    using System.Linq;
    using Application.Location;
    using Elastic.Common.IoC;
    using IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LocationLookupProviderTests
    {
        private Container _container;

        [SetUp]
        public void SetUp()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
            });
        }

        [Test, Category("Integration")]
        public void ShouldMatchExactPlaceName()
        {
            // arrange
            var service = _container.GetInstance<ILocationLookupProvider>();

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

            var service = _container.GetInstance<ILocationLookupProvider>();

            const string term = "Chellsmore";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Cheylesmore", results.First().Name);
        }

        [Test, Category("Integration")]
        public void ShouldMatchPlaceNamesWithSamePrefix()
        {
            // arrange
            var service = _container.GetInstance<ILocationLookupProvider>();

            const string term = "Coven";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            const int expectedPrefixMatchCount = 7;

            Assert.IsTrue(results.Count >= expectedPrefixMatchCount);

            foreach (var result in results.Take(expectedPrefixMatchCount))
            {
                StringAssert.StartsWith(term, result.Name);
            }
        }

        [Test, Category("Integration")]
        public void ShouldMatchPlaceNameWithUniquePrefix()
        {
            // arrange
            var service = _container.GetInstance<ILocationLookupProvider>();

            const string term = "Covent";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.IsTrue(results.Count > 1);
            StringAssert.StartsWith(term, results.First().Name);
        }
    }
}
