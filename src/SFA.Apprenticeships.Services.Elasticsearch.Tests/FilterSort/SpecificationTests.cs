using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;
using SFA.Apprenticeships.Services.Elasticsearch.Entities;
using SFA.Apprenticeships.Services.Elasticsearch.Specifications;
using SFA.Apprenticeships.Services.Elasticsearch.Tests.Models;

namespace SFA.Apprenticeships.Services.Elasticsearch.Tests.FilterSort
{
    [TestFixture]
    public class SpecificationTests
    {
        [TestCase]
        public void TermSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters {Title = new ElasticSortable<string> {Value = "Engineer"}};

            var spec = new TermSpecification<QueryParameters>(q => q.Title);

            spec.Build(query).Should().Be("{\"term\":{\"Title\":\"engineer\"}}");
        }

        [TestCase]
        public void TermSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters {Title = new ElasticSortable<string>()};

            var spec = new TermSpecification<QueryParameters>(q => q.Title);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void GeoLocationSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters { Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d } };

            var spec = new GeoLocationSpecification<QueryParameters>(q => q.Location);

            spec.Build(query).Should().Be("{\"geo_distance\":{\"distance\":\"20mi\",\"Location\":{\"lat\":52.79,\"lon\":-1.92}}}");
        }

        [TestCase]
        public void GeoLocationSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters { Title = new ElasticSortable<string>() };

            var spec = new GeoLocationSpecification<QueryParameters>(q => q.Location);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void RangeSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters {Wage = new Range<double> {RangeTo = 10d, RangeFrom = 1d}};

            var spec = new RangeSpecification<QueryParameters>(q => q.Wage);

            spec.Build(query).Should().Be("{\"range\":{\"Wage\":{\"from\":\"1\",\"to\":\"10\"}}}");
        }

        [TestCase]
        public void RangeSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters { Wage = new Range<double> {  } };

            var spec = new RangeSpecification<QueryParameters>(q => q.Wage);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void AndSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters
            {
                Title = new ElasticSortable<string> { SortEnabled = true, Value = "Engineer" },
                Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d },
            };

            var filterSpecs = new List<IConstraintSpecification<QueryParameters>>
            {
                new TermSpecification<QueryParameters>(q => q.Employer),
                new TermSpecification<QueryParameters>(q => q.Provider),
                new TermSpecification<QueryParameters>(q => q.Title),
                new TermSpecification<QueryParameters>(q => q.VacancyType),
                new RangeSpecification<QueryParameters>(q => q.Hours),
                new RangeSpecification<QueryParameters>(q => q.Wage),
                new GeoLocationSpecification<QueryParameters>(q => q.Location),
                new RangeSpecification<QueryParameters>(q => q.PostDate),
            };

            var spec = new AndSpecification<QueryParameters>(filterSpecs);

            var test = spec.Build(query);
            test.Should().Be("\"and\":[{\"term\":{\"Title\":\"engineer\"}},{\"geo_distance\":{\"distance\":\"20mi\",\"Location\":{\"lat\":52.79,\"lon\":-1.92}}}]");
        }

        [TestCase]
        public void AndSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters { Wage = new Range<double> { RangeTo = 10d, RangeFrom = 1d } };

            var spec = new AndSpecification<QueryParameters>(new List<IConstraintSpecification<QueryParameters>>());

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void SortByFieldnameSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters { Wage = new Range<double> { SortEnabled = true, SortDirection = SortDirectionType.Descending } };

            var spec = new SortByFieldnameSpecification<QueryParameters>(q => q.Wage);

            spec.Build(query).Should().Be("{\"Wage\":{\"order\":\"Descending\"}}");
        }

        [TestCase]
        public void SortByFieldnameSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters { Wage = new Range<double>{SortEnabled = false, SortDirection = SortDirectionType.Descending } };

            var spec = new SortByFieldnameSpecification<QueryParameters>(q => q.Wage);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void SortByLocationSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters { Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d } };

            var spec = new SortByLocationSpecification<QueryParameters>(q => q.Location);

            spec.Build(query).Should().Be("{\"_geo_distance\":{\"Location\":{\"lat\":52.79,\"lon\":-1.92},\"order\":\"asc\",\"unit\":\"mi\"}}");
        }

        [TestCase]
        public void SortByLocationSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters { Location = new SortableGeoLocation { SortEnabled = false } };

            var spec = new SortByLocationSpecification<QueryParameters>(q => q.Location);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void SortingSpecificationReturnsSortTerm()
        {
            var query = new QueryParameters
            {
                Title = new ElasticSortable<string> { SortEnabled = true, Value = "Engineer" },
                Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d },
            };

            var sortSpecs = new List<ISortableSpecification<QueryParameters>>
            {
                new SortByFieldnameSpecification<QueryParameters>(s => s.Postcode),
                new SortByFieldnameSpecification<QueryParameters>(s => s.Title),
                new SortByLocationSpecification<QueryParameters>(s => s.Location)
            };

            var spec = new SortingSpecification<QueryParameters>(sortSpecs);

            var test = spec.Build(query);
            test.Should().Be("{\"Title\":{\"order\":\"Ascending\"}},{\"_geo_distance\":{\"Location\":{\"lat\":52.79,\"lon\":-1.92},\"order\":\"asc\",\"unit\":\"mi\"}}");
        }

        [TestCase]
        public void SortingSpecificationReturnsEmptyString()
        {
            var query = new QueryParameters { Wage = new Range<double> { RangeTo = 10d, RangeFrom = 1d } };

            var spec = new SortingSpecification<QueryParameters>(new List<ISortableSpecification<QueryParameters>>());

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void ExampleQueryTest()
        {
            var queryParams = new QueryParameters
            {
                Title = new ElasticSortable<string> {SortEnabled = true, Value = "Engineer"},
                Location = new SortableGeoLocation {SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d},
            };

            var filterSpecs = new List<IConstraintSpecification<QueryParameters>>
            {
                new TermSpecification<QueryParameters>(q => q.Employer),
                new TermSpecification<QueryParameters>(q => q.Provider),
                new TermSpecification<QueryParameters>(q => q.Title),
                new TermSpecification<QueryParameters>(q => q.VacancyType),
                new RangeSpecification<QueryParameters>(q => q.Hours),
                new RangeSpecification<QueryParameters>(q => q.Wage),
                new GeoLocationSpecification<QueryParameters>(q => q.Location),
                new RangeSpecification<QueryParameters>(q => q.PostDate),
            };

            var sortSpecs = new List<ISortableSpecification<QueryParameters>>
            {
                new SortByFieldnameSpecification<QueryParameters>(s => s.Postcode),
                new SortByFieldnameSpecification<QueryParameters>(s => s.Title),
                new SortByLocationSpecification<QueryParameters>(s => s.Location)
            };

            var specs = new List<ISpecification<QueryParameters>>
            {
                new SortingSpecification<QueryParameters>(sortSpecs),
                new AndSpecification<QueryParameters>(filterSpecs)
            };

            var query = new QuerySpecification<QueryParameters>(specs).Build(queryParams);

            query
                .Should()
                .Be("{\"sort\":[{\"Title\":{\"order\":\"Ascending\"}},{\"_geo_distance\":{\"Location\":{\"lat\":52.79,\"lon\":-1.92},\"order\":\"asc\",\"unit\":\"mi\"}}],\"query\":{\"constant_score\":{\"filter\":{\"and\":[{\"term\":{\"Title\":\"engineer\"}},{\"geo_distance\":{\"distance\":\"20mi\",\"Location\":{\"lat\":52.79,\"lon\":-1.92}}}]}}}}");
        }
    }
}