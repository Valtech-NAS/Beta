namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Tests.Indexer
{
    using System;
    using System.Globalization;
    using System.Net;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using RestSharp;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Attributes;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Service;
    using StructureMap;

    [TestFixture]
    public class IndexerTests
    {
        [TestCase]
        public void ShouldBuildElasticsearchLoadAndGetMappingAttributes()
        {
            var service = new Mock<IElasticsearchService>();
            var test = new IndexingService<VacancySummary>(service.Object);

            test.Mapping.Should().NotBeNull();
            test.Mapping.Index.Should().Be("legacy");
            test.Mapping.Document.Should().Be("vacancy");
        }

        //[TestCase]
        //public void ShouldThrowExceptionForClassWithoutMappingAttributes()
        //{
        //    var service = new Mock<IElasticsearchService>();

        //    Action test = () => new IndexingService<VacancyId>(service.Object);

        //    test.ShouldThrow<ArgumentException>();
        //}

        [TestCase]
        public void ShouldThrowExceptionForClassWithMappingAttributeWithoutDocumentAttribute()
        {
            var service = new Mock<IElasticsearchService>();

            Action test = () => new IndexingService<TestClassNoIndex>(service.Object);

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void ShouldThrowExceptionForClassWithMappingAttributeWithoutIndexAttribute()
        {
            var service = new Mock<IElasticsearchService>();

            Action test = () => new IndexingService<TestClassNoDocument>(service.Object);

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void ShouldUseServiceToSendVacancySummary()
        {
            var summary = new VacancySummary
            {
                Id = 1,
            };

            var rs = new Mock<IRestResponse>();
            rs.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);

            var service = new Mock<IElasticsearchService>();
            service.Setup(x => x.Execute("legacy", "vacancy", summary.Id.ToString(CultureInfo.InvariantCulture), It.IsAny<string>())).Returns(rs.Object);

            var loader = new IndexingService<VacancySummary>(service.Object);

            loader.Index("", summary);

            service.Verify(x => x.Execute("legacy", "vacancy", summary.Id.ToString(CultureInfo.InvariantCulture), It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void SetupShouldCreateIndexAndMappingsRequests()
        {
            var rs = new Mock<IRestResponse>();
            rs.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);

            var service = new Mock<IElasticsearchService>();
            service.Setup(x => x.Execute(Method.PUT, "legacy")).Returns(rs.Object);
            service.Setup(x => x.Execute("legacy", "vacancy", "_mapping", It.IsAny<string>())).Returns(rs.Object);

            ObjectFactory.Initialize(x => x.For<IElasticsearchService>().Use(service.Object));

            var indexing = new IndexingService<VacancySummary>(service.Object);

            service.Verify(x => x.Execute(Method.PUT, "legacy"), Times.Once);
            service.Verify(x => x.Execute("legacy", "vacancy", "_mapping", It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void SetupShouldCreateIndexAndMappingsRequestsIfIndexAlreadyExists()
        {
            var rsOk = new Mock<IRestResponse>();
            rsOk.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
            var rsBad = new Mock<IRestResponse>();
            rsBad.Setup(x => x.StatusCode).Returns(HttpStatusCode.BadRequest);
            rsBad.Setup(x => x.Content).Returns("IndexAlreadyExistsException");

            var service = new Mock<IElasticsearchService>();
            service.Setup(x => x.Execute(Method.PUT, "legacy")).Returns(rsBad.Object);
            service.Setup(x => x.Execute("legacy", "vacancy", "_mapping", It.IsAny<string>())).Returns(rsOk.Object);

            ObjectFactory.Initialize(x => x.For<IElasticsearchService>().Use(service.Object));

            var indexing = new IndexingService<VacancySummary>(service.Object);

            service.Verify(x => x.Execute(Method.PUT, "legacy"), Times.Once);
            service.Verify(x => x.Execute("legacy", "vacancy", "_mapping", It.IsAny<string>()), Times.Once);
        }
    }

    [ElasticsearchMapping(Index = "indexonly")]
    class TestClassNoDocument
    {        
    }

    [ElasticsearchMapping(Document = "documentonly")]
    class TestClassNoIndex
    {
    }
}
