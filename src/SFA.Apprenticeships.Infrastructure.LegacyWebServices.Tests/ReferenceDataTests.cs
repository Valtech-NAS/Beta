namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Application.ReferenceData;
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

        [TestCase("Counties", 46)]
        public void GetApprenticeshipFrameworksShouldReturnList(string referenceDataType, int numberReturned)
        {
            var test = _service.GetReferenceData(referenceDataType);
            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(numberReturned);
        }

        /// <summary>
        /// TODO: remove and test cache without specifically linking to a service / provider.
        /// </summary>
        [TestCase]
        public void ShouldGetServiceResponseFromCache()
        {
            var cache = ObjectFactory.GetInstance<ICacheService>();
            cache.FlushAll();

            // call once to fill cache.
            _service.GetReferenceData("Counties");

            // Mock the service to ensure it's not called but use the same cache.
            var uncachedServicce = new Mock<IReferenceDataService>();
            uncachedServicce.Setup(x => x.GetReferenceData(It.IsAny<string>())).Throws<InvalidOperationException>();

            var service = new CachedReferenceDataService(cache, uncachedServicce.Object);
            var test = service.GetReferenceData("Counties");

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(46);
        }
    }
}
