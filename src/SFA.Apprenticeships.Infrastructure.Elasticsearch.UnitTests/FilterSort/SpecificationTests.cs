namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.UnitTests.FilterSort
{
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Elasticsearch;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Specifications;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.UnitTests.Models;

    [TestFixture]
    public class SpecificationTests
    {
        [TestCase]
        public void TermSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel {Title = new Sortable<string> {Value = "Engineer"}};

            var spec = new TermSpecification<QueryTestModel>(q => q.Title);

            spec.Build(query).Should().Be("{\"term\":{\"Title\":\"engineer\"}}");
        }

        [TestCase]
        public void TermSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel {Title = new Sortable<string>()};

            var spec = new TermSpecification<QueryTestModel>(q => q.Title);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void GeoLocationSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel { Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d } };

            var spec = new GeoLocationSpecification<QueryTestModel>(q => q.Location);

            spec.Build(query).Should().Be("{\"geo_distance\":{\"distance\":\"20mi\",\"Location\":{\"lat\":52.79,\"lon\":-1.92}}}");
        }

        [TestCase]
        public void GeoLocationSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel { Title = new Sortable<string>() };

            var spec = new GeoLocationSpecification<QueryTestModel>(q => q.Location);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void RangeSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel {Wage = new SortableRange<double> {RangeTo = 10d, RangeFrom = 1d}};

            var spec = new RangeSpecification<QueryTestModel>(q => q.Wage);

            spec.Build(query).Should().Be("{\"range\":{\"Wage\":{\"from\":\"1\",\"to\":\"10\"}}}");
        }

        [TestCase]
        public void RangeSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel { Wage = new SortableRange<double> {  } };

            var spec = new RangeSpecification<QueryTestModel>(q => q.Wage);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void AndSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel
            {
                Title = new Sortable<string> { SortEnabled = true, Value = "Engineer" },
                Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d },
            };

            var filterSpecs = new List<IConstraintSpecification<QueryTestModel>>
            {
                new TermSpecification<QueryTestModel>(q => q.Employer),
                new TermSpecification<QueryTestModel>(q => q.Provider),
                new TermSpecification<QueryTestModel>(q => q.Title),
                new TermSpecification<QueryTestModel>(q => q.VacancyType),
                new RangeSpecification<QueryTestModel>(q => q.Hours),
                new RangeSpecification<QueryTestModel>(q => q.Wage),
                new GeoLocationSpecification<QueryTestModel>(q => q.Location),
                new RangeSpecification<QueryTestModel>(q => q.PostDate),
            };

            var spec = new AndSpecification<QueryTestModel>(filterSpecs);

            var test = spec.Build(query);
            test.Should().Be("\"and\":[{\"term\":{\"Title\":\"engineer\"}},{\"geo_distance\":{\"distance\":\"20mi\",\"Location\":{\"lat\":52.79,\"lon\":-1.92}}}]");
        }

        [TestCase]
        public void AndSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel { Wage = new SortableRange<double> { RangeTo = 10d, RangeFrom = 1d } };

            var spec = new AndSpecification<QueryTestModel>(new List<IConstraintSpecification<QueryTestModel>>());

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void SortByFieldnameSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel { Wage = new SortableRange<double> { SortEnabled = true, SortDirection = SortDirectionType.Descending } };

            var spec = new SortByFieldnameSpecification<QueryTestModel>(q => q.Wage);

            spec.Build(query).Should().Be("{\"Wage\":{\"order\":\"desc\"}}");
        }

        [TestCase]
        public void SortByFieldnameSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel { Wage = new SortableRange<double>{SortEnabled = false, SortDirection = SortDirectionType.Descending } };

            var spec = new SortByFieldnameSpecification<QueryTestModel>(q => q.Wage);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void SortByLocationSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel { Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d } };

            var spec = new SortByLocationSpecification<QueryTestModel>(q => q.Location);

            spec.Build(query).Should().Be("{\"_geo_distance\":{\"Location\":{\"lat\":52.79,\"lon\":-1.92},\"order\":\"asc\",\"unit\":\"mi\"}}");
        }

        [TestCase]
        public void SortByLocationSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel { Location = new SortableGeoLocation { SortEnabled = false } };

            var spec = new SortByLocationSpecification<QueryTestModel>(q => q.Location);

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void SortingSpecificationReturnsSortTerm()
        {
            var query = new QueryTestModel
            {
                Title = new Sortable<string> { SortEnabled = true, Value = "Engineer" },
                Location = new SortableGeoLocation { SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d },
            };

            var sortSpecs = new List<ISortableSpecification<QueryTestModel>>
            {
                new SortByFieldnameSpecification<QueryTestModel>(s => s.Postcode),
                new SortByFieldnameSpecification<QueryTestModel>(s => s.Title) {SortOrder = 2},
                new SortByLocationSpecification<QueryTestModel>(s => s.Location) {SortOrder = 1}
            };

            var spec = new SortingSpecification<QueryTestModel>(sortSpecs);

            var test = spec.Build(query);
            test.Should().Be("{\"_geo_distance\":{\"Location\":{\"lat\":52.79,\"lon\":-1.92},\"order\":\"asc\",\"unit\":\"mi\"}},{\"Title\":{\"order\":\"asc\"}}");
        }

        [TestCase]
        public void SortingSpecificationReturnsEmptyString()
        {
            var query = new QueryTestModel { Wage = new SortableRange<double> { RangeTo = 10d, RangeFrom = 1d } };

            var spec = new SortingSpecification<QueryTestModel>(new List<ISortableSpecification<QueryTestModel>>());

            spec.Build(query).Should().Be("");
        }

        [TestCase]
        public void ExampleQueryTest()
        {
            var queryParams = new QueryTestModel
            {
                Title = new Sortable<string> {SortEnabled = true, Value = "Engineer"},
                Location = new SortableGeoLocation {SortEnabled = true, Distance = 20d, lat = 52.79d, lon = -1.92d},
            };

            var filterSpecs = new List<IConstraintSpecification<QueryTestModel>>
            {
                new TermSpecification<QueryTestModel>(q => q.Employer),
                new TermSpecification<QueryTestModel>(q => q.Provider),
                new TermSpecification<QueryTestModel>(q => q.Title),
                new TermSpecification<QueryTestModel>(q => q.VacancyType),
                new RangeSpecification<QueryTestModel>(q => q.Hours),
                new RangeSpecification<QueryTestModel>(q => q.Wage),
                new GeoLocationSpecification<QueryTestModel>(q => q.Location),
                new RangeSpecification<QueryTestModel>(q => q.PostDate),
            };

            var sortSpecs = new List<ISortableSpecification<QueryTestModel>>
            {
                new SortByFieldnameSpecification<QueryTestModel>(s => s.Postcode),
                new SortByFieldnameSpecification<QueryTestModel>(s => s.Title) {SortOrder = 1},
                new SortByLocationSpecification<QueryTestModel>(s => s.Location) {SortOrder = 2}
            };

            var specs = new List<ISpecification<QueryTestModel>>
            {
                new SortingSpecification<QueryTestModel>(sortSpecs),
                new AndSpecification<QueryTestModel>(filterSpecs)
            };

            var query = new QuerySpecification<QueryTestModel>(specs).Build(queryParams);

            query
                .Should()
                .Be("{\"sort\":[{\"Title\":{\"order\":\"asc\"}},{\"_geo_distance\":{\"Location\":{\"lat\":52.79,\"lon\":-1.92},\"order\":\"asc\",\"unit\":\"mi\"}}],\"query\":{\"constant_score\":{\"filter\":{\"and\":[{\"term\":{\"Title\":\"engineer\"}},{\"geo_distance\":{\"distance\":\"20mi\",\"Location\":{\"lat\":52.79,\"lon\":-1.92}}}]}}}}");
        }
    }
}