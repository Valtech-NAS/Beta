namespace SFA.Apprenticeships.Services.ReferenceData.IntegrationTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration.LegacyServices;
    using SFA.Apprenticeships.Common.Interfaces.Enums.ReferenceDataService;
    using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Services.Common.Wcf;

    using SFA.Apprenticeships.Services.ReferenceData.Proxy;
    using StructureMap;

    [TestFixture]
    public class ReferenceDataTests
    {
        private IReferenceDataService _service;
        private ILegacyServicesConfiguration _legacyServicesConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {
            Apprenticeships.Common.IoC.IoC.Initialize();
            _legacyServicesConfiguration = ObjectFactory.GetInstance<ILegacyServicesConfiguration>();
            _service = ObjectFactory.GetInstance<IReferenceDataService>();
        }

        [TestCase]
        public void TheServiceEndpointShouldRespond()
        {
            var result = default(GetApprenticeshipFrameworksResponse);

            var msgId = new Guid();

            var rq = new GetApprenticeshipFrameworksRequest(_legacyServicesConfiguration.SystemId, msgId, _legacyServicesConfiguration.PublicKey);
            var service = new WcfService<IReferenceData>();
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

        [TestCase(LegacyReferenceDataType.County)]
        [TestCase(LegacyReferenceDataType.ErrorCode)]
        [TestCase(LegacyReferenceDataType.Framework)]
        [TestCase(LegacyReferenceDataType.LocalAuthority)]
        [TestCase(LegacyReferenceDataType.Occupations)]
        [TestCase(LegacyReferenceDataType.Region)]
        public void GetReferenceDataShouldReturnCollection(LegacyReferenceDataType type)
        {
            var test = _service.GetReferenceData(type);

            test.Should().NotBeNullOrEmpty();
        }
    }
}
