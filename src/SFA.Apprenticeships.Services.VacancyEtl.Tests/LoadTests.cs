using System;
using System.Globalization;
using System.Net;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestSharp;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.VacancyEtl.Load;
using StructureMap;

namespace SFA.Apprenticeships.Services.VacancyEtl.Tests
{
    [TestFixture]
    public class LoadTests
    {
        [TestCase]
        public void ShouldBuildElasticsearchLoadAndGetMappingAttributes()
        {
            var service = new Mock<IElasticsearchService>();

            var test = new ElasticsearchLoad<VacancySummary>(service.Object);

            test.Mapping.Should().NotBeNull();
            test.Mapping.Index.Should().Be("legacy");
            test.Mapping.Document.Should().Be("vacancy");
        }

        [TestCase]
        public void ShouldThrowExceptionForClassWithoutMappingAttributes()
        {
            var service = new Mock<IElasticsearchService>();

            Action test = () => new ElasticsearchLoad<VacancyId>(service.Object);

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void ShouldThrowExceptionForClassWithMappingAttributeWithoutDocumentAttribute()
        {
            var service = new Mock<IElasticsearchService>();

            Action test = () => new ElasticsearchLoad<TestClassNoIndex>(service.Object);

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void ShouldThrowExceptionForClassWithMappingAttributeWithoutIndexAttribute()
        {
            var service = new Mock<IElasticsearchService>();

            Action test = () => new ElasticsearchLoad<TestClassNoDocument>(service.Object);

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

            var loader = new ElasticsearchLoad<VacancySummary>(service.Object);

            loader.Execute(summary);

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

            ElasticsearchLoad<VacancySummary>.Setup();

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

            ElasticsearchLoad<VacancySummary>.Setup();

            service.Verify(x => x.Execute(Method.PUT, "legacy"), Times.Once);
            service.Verify(x => x.Execute("legacy", "vacancy", "_mapping", It.IsAny<string>()), Times.Once);
        }
    }

    [ElasticsearchMapping(Index = "indexonly")]
    class TestClassNoDocument : VacancyId
    {        
    }

    [ElasticsearchMapping(Document = "documentonly")]
    class TestClassNoIndex : VacancyId
    {
    }
}
