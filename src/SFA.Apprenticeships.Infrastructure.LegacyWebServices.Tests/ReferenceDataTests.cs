using SFA.Apprenticeships.Application.ReferenceData;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;
    using SFA.Apprenticeships.Infrastructure.Common.Wcf;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceDataProxy;
    using StructureMap;

    [TestFixture]
    public class ReferenceDataTests
    {
        private IReferenceDataService _service;
        private ILegacyServicesConfiguration _legacyServicesConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {
            _legacyServicesConfiguration = ObjectFactory.GetInstance<ILegacyServicesConfiguration>();
            _service = ObjectFactory.GetInstance<IReferenceDataService>();
        }

        [TestCase]
        public void TheServiceEndpointShouldRespond()
        {
            var result = default(GetApprenticeshipFrameworksResponse);

            var msgId = new Guid();

            var rq = new GetApprenticeshipFrameworksRequest(_legacyServicesConfiguration.SystemId, msgId, _legacyServicesConfiguration.PublicKey);
            var service = ObjectFactory.GetInstance<IWcfService<IReferenceData>>();
            service.Use(client=> { result = client.GetApprenticeshipFrameworks(rq); });

            result.Should().NotBeNull();
        }

        [TestCase]
        public void GetApprenticeshipFrameworksShouldReturnList()
        {       
            var test = _service.GetApprenticeshipFrameworks();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(217);
        }

        [TestCase]
        public void GetCountiesShouldReturnList()
        {
            var test = _service.GetCounties();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(46);
        }

        [TestCase]
        public void GetErrorCodesShouldReturnList()
        {
            var test = _service.GetErrorCodes();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(72);
        }

        [TestCase]
        public void GetLocalAuthoritiesShouldReturnList()
        {
            var test = _service.GetLocalAuthorities();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(326);
        }

        [TestCase]
        public void GetRegionsShouldReturnList()
        {
            var test = _service.GetRegions();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(10);
        }

        [Test]
        public void GetErrorCodesShouldReturnCollection()
        {
            var errors = _service.GetErrorCodes();
            errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetCountiesShouldReturnCollection()
        {
            var counties = _service.GetCounties();
            counties.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetRegionsShouldReturnCollection()
        {
            var regions = _service.GetRegions();
            regions.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetLocalAuthsShouldReturnCollection()
        {
            var localAuths = _service.GetLocalAuthorities();
            localAuths.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetApprenticeshipFrameworksReturnCollection()
        {
            var apprenticeshipFrameworks = _service.GetApprenticeshipFrameworks();
            apprenticeshipFrameworks.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetApprenticeshipOccupationsReturnCollection()
        {
            var apprenticeshipOccupations = _service.GetApprenticeshipOccupations();
            apprenticeshipOccupations.Should().NotBeNullOrEmpty();
        }

        [TestCase]
        public void ShouldGetServiceResponseFromCache()
        {
            var cache = ObjectFactory.GetInstance<ICacheService>();
            cache.FlushAll();

            // call once to fill cache.
            _service.GetApprenticeshipFrameworks();

            // Mock the service to ensure it's not called but use the same cache.
            var uncachedServicce = new Mock<IReferenceDataService>();
            uncachedServicce.Setup(x => x.GetApprenticeshipFrameworks()).Throws<InvalidOperationException>();

            var service = new CachedReferenceDataService(cache, uncachedServicce.Object);
            var test = service.GetApprenticeshipFrameworks();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(217);
        }
    }
}
