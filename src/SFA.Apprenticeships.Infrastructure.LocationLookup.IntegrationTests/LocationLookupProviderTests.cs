namespace SFA.Apprenticeships.Infrastructure.LocationLookup.IntegrationTests
{
    using System.Linq;
    using Application.Location;
    using Elastic.Common.IoC;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LocationLookupProviderTests
    {
        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
            });
#pragma warning restore 0618
        }

        [Test, Category("Integration")]
        public void ShouldMatchExactPlaceName()
        {
            // arrange
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
#pragma warning restore 0618

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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
#pragma warning restore 0618

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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
#pragma warning restore 0618

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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<ILocationLookupProvider>();
#pragma warning restore 0618

            const string term = "Covent";

            // act
            var results = service.FindLocation(term).ToList();

            // assert
            Assert.IsTrue(results.Count > 1);
            StringAssert.StartsWith(term, results.First().Name);
        }
    }
}
