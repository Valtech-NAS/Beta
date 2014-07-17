namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using System.Linq;
    using Caching.Memory.IoC;
    using Common.IoC;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using IoC;
    using Moq;
    using NUnit.Framework;
    using Application.Interfaces.ReferenceData;
    using Application.ReferenceData;
    using Domain.Interfaces.Caching;
    using Configuration;
    using ReferenceDataProxy;
    using StructureMap;
    using Wcf;

    [TestFixture]
    public class ReferenceDataTests
    {
        private IReferenceDataService _service;
        private ILegacyServicesConfiguration _legacyServicesConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.For<IReferenceDataService>().Use<ReferenceDataService>();
            });

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
            var items = test as ReferenceDataItem[] ?? test.ToArray();
            items.Should().NotBeNullOrEmpty();
            items.Count().Should().Be(numberReturned);
        }

        /// <summary>
        /// TODO: DONTKNOW: remove and test cache without specifically linking to a service / provider.
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

            var items = test as ReferenceDataItem[] ?? test.ToArray();
            items.Should().NotBeNullOrEmpty();
            items.Count().Should().Be(46);
        }
    }
}
