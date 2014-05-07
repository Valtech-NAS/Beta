using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.Helpers;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;
using SFA.Apprenticeships.Services.Elasticsearch.Entities;
using SFA.Apprenticeships.Services.Elasticsearch.Filtering;

namespace SFA.Apprenticeships.Services.Elasticsearch.Tests.FilterSort
{
    [TestFixture]
    public class SortTests
    {
        [TestCase("desc")]
        [TestCase("asc")]
        public void CreateOrderedSortTerm(string order)
        {
            var term = new TestSort
            {
                SortBy = SortByType.Fieldname,
                SortDirection = order.GetEnumFromDescription<SortDirectionType>(),
                SortFieldname = "Test"
            };

            var filter = new Sort<TestSort>(term);
            var test = filter.Create();

            test.Should().Be(string.Format("\"sort\":[{{\"Test\":{{\"order\":\"{0}\"}}}}]", order));
        }

        [TestCase("desc")]
        [TestCase("asc")]
        public void CreateOrderedGeoSortTerm(string order)
        {
            var term = new TestSort
            {
                SortBy = SortByType.Point,
                SortDirection = order.GetEnumFromDescription<SortDirectionType>(),
                SortFieldname = "Test",
                Location = new GeoLocation {Distance = 1d, HasValue = true, lat = 2d, lon = 3d}
            };

            var filter = new Sort<TestSort>(term);
            var test = filter.Create();

            test.Should()
                .Be(
                    string.Format(
                        "\"sort\":[{{\"_geo_distance\":{{\"Test\":{{\"lat\":2,\"lon\":3}},\"order\":\"{0}\",\"unit\":\"mi\"}}}}]",
                        order));
        }
    }

    internal class TestSort : ISortLocation
    {
        public SortByType SortBy { get; set; }
        public SortDirectionType SortDirection { get; set; }
        public string SortFieldname { get; set; }
        public GeoLocation Location { get; set; }
    }
}