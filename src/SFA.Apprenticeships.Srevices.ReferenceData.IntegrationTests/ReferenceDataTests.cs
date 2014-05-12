using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Services.Common.Wcf;
using SFA.Apprenticeships.Services.ReferenceData.Proxy;

namespace SFA.Apprenticeships.Srevices.ReferenceData.IntegrationTests
{
    [TestFixture]
    public class ReferenceDataTests
    {
        [TestCase]
        [Ignore("Need to sort out how to use protected settings")]
        public void DoesTheEndpointRespond()
        {
            var filename = string.Format(
                "{0}{1}", 
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\App_Data\private.settings.debug.config");

            var configManager =
                new ConfigurationManager(new ConfigurationSettingsService(filename, ConfigurationSettingsType.File));
            
            var result = default(GetApprenticeshipFrameworksResponse);

            var msgId = new Guid();
            var username = configManager.GetAppSetting("ReferenceDataService.Username");
            var password = configManager.GetAppSetting("ReferenceDataService.Password");

            var rq = new GetApprenticeshipFrameworksRequest(new Guid(username), msgId, password);

            Service<IReferenceData>.Use(client=> { result = client.GetApprenticeshipFrameworks(rq); });

            result.Should().NotBeNull();
        }
    }
}
