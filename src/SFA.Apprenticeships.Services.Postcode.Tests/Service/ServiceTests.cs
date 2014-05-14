using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestSharp;
using SFA.Apprenticeships.Services.Postcode.Entities;
using SFA.Apprenticeships.Services.Postcode.Service;

namespace SFA.Apprenticeships.Services.Postcode.Tests.Service
{
    [TestFixture]
    public class ServiceTests
    {
        [TestCase("")]
        [TestCase(null)]
        public void DoesConstructorThrowExceptionWithNoUrl(string url)
        {
            Action test = () => new PostcodeService(url);

            test.ShouldThrow<ArgumentNullException>();
        }

        [TestCase]
        public void CanGetDefaultRestClientWithBaseUrl()
        {
            var service = new PostcodeService("http://api.postcodes.io");

            service.Client.BaseUrl.Should().Be("http://api.postcodes.io");
        }

        [TestCase]
        public void GetRandomPostcodeReturnsResult()
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

            var restClient = new Mock<IRestClient>();
            restClient.Setup(x=>x.Execute<PostcodeInfoResult>(It.IsAny<IRestRequest>())).Returns(response);

            var service = new PostcodeService("http://api.postcodes.io") { Client = restClient.Object };

            service.GetRandomPostcode().Postcode.Should().Be("CV1 2WT");
        }

        [TestCase]
        public void GetPartialMatchReturnsResult()
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
                        },
                         new PostcodeInfo
                        {
                            Postcode = "CV1 4WT"
                        }
                    }
                }
            };

            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<PostcodeInfoResult>(It.IsAny<IRestRequest>())).Returns(response);

            var service = new PostcodeService("http://api.postcodes.io") { Client = restClient.Object };

            var result = service.GetPartialMatches("any");

            result.Count.Should().Be(2);
            result.First().Postcode.Should().Be("CV1 2WT");
        }

        [TestCase]
        public void GetPostcodeReturnsResult()
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
                        },
                         new PostcodeInfo
                        {
                            Postcode = "CV1 4WT"
                        }
                    }
                }
            };

            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<PostcodeInfoResult>(It.IsAny<IRestRequest>())).Returns(response);

            var service = new PostcodeService("http://api.postcodes.io") { Client = restClient.Object };

            var result = service.GetPostcodeInfo("any");

            result.Postcode.Should().Be("CV1 2WT");
        }

        [TestCase]
        public void ServiceThrowsRestClientException()
        {
            var response = new RestResponse<PostcodeInfoResult>
            {
                ErrorException = new Exception("Test")
            };

            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<PostcodeInfoResult>(It.IsAny<IRestRequest>())).Returns(response);

            var service = new PostcodeService("http://api.postcodes.io") { Client = restClient.Object };

            Action test = () => service.GetRandomPostcode();

            test.ShouldThrow<ApplicationException>();
        }

        [TestCase(Description = "Checks the implementation and will fail to remind to build test when the service is implemented")]
        public void GetPostcodeFromLatLongThrowsException()
        {
            Action test = () =>
            {
                var service = new PostcodeService("http://api.postcodes.io");
                service.GetPostcodeFromLatLong(string.Empty);
            };

            test.ShouldThrow<NotImplementedException>();
        }

        [TestCase(Description = "Checks the implementation and will fail to remind to build test when the service is implemented")]
        public void ValidatePostcodeThrowsException()
        {
            Action test = () =>
            {
                var service = new PostcodeService("http://api.postcodes.io");
                service.ValidatePostcode(string.Empty);
            };

            test.ShouldThrow<NotImplementedException>();
        }
    }
}
