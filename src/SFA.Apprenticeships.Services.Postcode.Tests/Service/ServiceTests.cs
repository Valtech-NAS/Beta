using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RestSharp;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Services.Postcode.Entities;
using SFA.Apprenticeships.Services.Postcode.Service;

namespace SFA.Apprenticeships.Services.Postcode.Tests.Service
{
    [TestFixture]
    public class ServiceTests
    {
        [TestCase]
        public void DoesConstructorThrowExceptionWithNoConfig()
        {
            Action test = () => new PostcodeService(default(IConfigurationManager));

            test.ShouldThrow<ArgumentNullException>();
        }

        [TestCase]
        public void CanGetDefaultRestClientWithBaseUrl()
        {
            var configManager = Substitute.For<IConfigurationManager>();
            configManager.GetAppSetting("PostcodeServiceEndpoint").Returns("http://api.postcodes.io");

            var service = new PostcodeService(configManager);

            service.Client.BaseUrl.Should().Be("http://api.postcodes.io");
        }

        [TestCase]
        public void GetRandomPasswordReturnsResult()
        {
            var response = new RestResponse<PostcodeInfoResult>
            {
                Data = new PostcodeInfoResult
                {
                    Result = new List<PostcodeInfo>
                    {
                        new PostcodeInfo
                        {
                            Postcode = "CV1 2WT"
                        }
                    }
                }
            };

            var configManager = Substitute.For<IConfigurationManager>();
            configManager.GetAppSetting("PostcodeServiceEndpoint").Returns("http://api.postcodes.io");

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<PostcodeInfoResult>(Arg.Any<IRestRequest>()).Returns(response);

            var service = new PostcodeService(configManager) {Client = restClient};

            service.GetRandomPostcode().Postcode.Should().Be("CV1 2WT");
        }

        [TestCase]
        public void ServiceThrowsRestClientException()
        {
            var response = new RestResponse<PostcodeInfoResult>
            {
                ErrorException = new Exception("Test")
            };

            var configManager = Substitute.For<IConfigurationManager>();
            configManager.GetAppSetting("PostcodeServiceEndpoint").Returns("http://api.postcodes.io");

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<PostcodeInfoResult>(Arg.Any<IRestRequest>()).Returns(response);

            var service = new PostcodeService(configManager) { Client = restClient };

            Action test = () => service.GetRandomPostcode();

            test.ShouldThrow<ApplicationException>();
        }
    }
}
