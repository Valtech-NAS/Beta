using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Common.Configuration;
using SFA.Apprenticeships.Services.Common.Wcf;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Proxy;
using SFA.Apprenticeships.Services.ReferenceData.Service;

namespace SFA.Apprenticeships.Services.ReferenceData.IntegrationTests
{
    [TestFixture]
    public class ReferenceDataTests
    {
        private IReferenceDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            var configManager = new ConfigurationManager();
            var wcf = new WcfService<IReferenceData>();
            _service = new ReferenceDataService(configManager, wcf);
        }

        [TestCase]
        public void DoesTheEndpointRespond()
        {
           var configManager = new ConfigurationManager();
            
            var result = default(GetApprenticeshipFrameworksResponse);

            var msgId = new Guid();
            var username = configManager.GetAppSetting("ReferenceDataService.Username");
            var password = configManager.GetAppSetting("ReferenceDataService.Password");

            var rq = new GetApprenticeshipFrameworksRequest(new Guid(username), msgId, password);
            var service = new WcfService<IReferenceData>();
            service.Use(client=> { result = client.GetApprenticeshipFrameworks(rq); });

            result.Should().NotBeNull();
        }

        [TestCase]
        public void GetApprenticeshipFrameworksReturnsList()
        {       
            var test = _service.GetApprenticeshipFrameworks();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(216);
        }

        [TestCase]
        public void GetCountiesReturnsList()
        {
            var test = _service.GetCounties();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(46);
        }

        [TestCase]
        public void GetErrorCodesReturnsList()
        {
            var test = _service.GetErrorCodes();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(72);
        }

        [TestCase]
        public void GetLocalAuthoritiesReturnsList()
        {
            var test = _service.GetLocalAuthorities();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(326);
        }

        [TestCase]
        public void GetRegionsReturnsList()
        {
            var test = _service.GetRegions();

            test.Should().NotBeNullOrEmpty();
            test.Count().Should().Be(10);
        }
    }
}
