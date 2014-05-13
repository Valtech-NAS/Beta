using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Services.Common.Wcf;
using SFA.Apprenticeships.Services.ReferenceData.Proxy;
using SFA.Apprenticeships.Services.ReferenceData.Service;

namespace SFA.Apprenticeships.Services.ReferenceData.IntegrationTests
{
    [TestFixture]
    public class ReferenceDataTests
    {
        private string _filename;

        [TestFixtureSetUp]
        public void Setup()
        {
            _filename =
                System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSetting];
        }

        [TestCase]
        public void DoesTheEndpointRespond()
        {
           var configManager =
                new ConfigurationManager(_filename);
            
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
            var configManager = new ConfigurationManager(_filename);
            var wcf = new WcfService<IReferenceData>();

            var service = new ReferenceDataService(configManager, wcf);
            var test = service.GetApprenticeshipFrameworks();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(216);
        }

        [TestCase]
        public void GetCountiesReturnsList()
        {
            var configManager = new ConfigurationManager(_filename);
            var wcf = new WcfService<IReferenceData>();

            var service = new ReferenceDataService(configManager, wcf);
            var test = service.GetCounties();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(46);
        }

        [TestCase]
        public void GetErrorCodesReturnsList()
        {
            var configManager = new ConfigurationManager(_filename);
            var wcf = new WcfService<IReferenceData>();

            var service = new ReferenceDataService(configManager, wcf);
            var test = service.GetErrorCodes();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(72);
        }

        [TestCase]
        public void GetLocalAuthoritiesReturnsList()
        {
            var configManager = new ConfigurationManager(_filename);
            var wcf = new WcfService<IReferenceData>();

            var service = new ReferenceDataService(configManager, wcf);
            var test = service.GetLocalAuthorities();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(326);
        }

        [TestCase]
        public void GetRegionsReturnsList()
        {
            var configManager = new ConfigurationManager(_filename);
            var wcf = new WcfService<IReferenceData>();

            var service = new ReferenceDataService(configManager, wcf);
            var test = service.GetRegions();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(10);
        }
    }
}
