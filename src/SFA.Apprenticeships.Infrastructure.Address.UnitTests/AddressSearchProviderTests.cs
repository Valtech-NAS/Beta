using System.Linq;
using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.Address.Mappers;
using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;

namespace SFA.Apprenticeships.Infrastructure.Address.UnitTests
{
    [TestFixture]
    public class AddressSearchProviderTests
    {
        [TestCase("O0 0OO", 0)]
        [TestCase("N7 8LS", 1)]
        [TestCase("n7 8LS", 1)]
        [TestCase("n7 8ls", 1)]
        [TestCase(" N7 8LS", 1)]
        [TestCase("N7 8LS ", 1)]
        [TestCase("N78LS", 1)]
        [TestCase("n78LS", 1)]
        [TestCase("n78ls", 1)]
        [TestCase(" N78LS", 1)]
        [TestCase("N78LS ", 1)]
        [TestCase("N 7 8 L S", 1)]
        public void TestPostcode(string postcode, int expectedResultCount)
        {
            var clientFactory = new ElasticsearchClientFactory(ElasticsearchConfiguration.Instance, false);
            var mapper = new AddressMapper();
            var addressSearchProvider = new AddressSearchProvider(clientFactory, mapper);
            var addresses = addressSearchProvider.FindAddress(postcode).ToList();
            Assert.That(addresses.Count, Is.EqualTo(expectedResultCount));
        }
    }
}