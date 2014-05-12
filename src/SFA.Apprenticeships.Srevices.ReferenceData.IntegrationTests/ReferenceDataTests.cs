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
            var config = new ConfigurationManager();
            var path = config.GetAppSetting("ConfigurationPath");
            var file = config.GetAppSetting("Settings");
            _filename = string.Format("{0}\\{1}", path, file);
        }

        [TestCase]
        public void DoesTheEndpointRespond()
        {
           var configManager =
                new ConfigurationManager(new ConfigurationSettingsService(_filename, ConfigurationSettingsType.File));
            
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
                new ConfigurationManager(new ConfigurationSettingsService(_filename, ConfigurationSettingsType.File));

            var service = new ReferenceDataService(configManager);
            var test = service.GetApprenticeshipFrameworks();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(216);
        }

        [TestCase]
        public void GetApprenticeshipOccupationsReturnsList()
        {
            var configManager =
                new ConfigurationManager(new ConfigurationSettingsService(_filename, ConfigurationSettingsType.File));

            var service = new ReferenceDataService(configManager);
            var test = service.GetApprenticeshipOccupations();

            test.Should().NotBeNullOrEmpty();
            test.Count.Should().Be(216);
        }
    }
}
