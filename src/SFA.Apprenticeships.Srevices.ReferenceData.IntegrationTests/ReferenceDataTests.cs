using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Services.Common.Wcf;
using SFA.Apprenticeships.Services.ReferenceData.Proxy;
using SFA.Apprenticeships.Services.ReferenceData.Service;

namespace SFA.Apprenticeships.Srevices.ReferenceData.IntegrationTests
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

            WcfService<IReferenceData>.Use(client=> { result = client.GetApprenticeshipFrameworks(rq); });

            result.Should().NotBeNull();
        }

        [TestCase]
        public void GetApprenticeshipFrameworksReturnsList()
        {
            var configManager =
                new ConfigurationManager(_filename);

            var service = new ReferenceDataService(configManager);
            var test = service.GetApprenticeshipFrameworks();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(216);
        }

        [TestCase]
        public void GetCountiesReturnsList()
        {
            var configManager =
                new ConfigurationManager(_filename);

            var service = new ReferenceDataService(configManager);
            var test = service.GetCounties();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(46);
        }
    }
}
