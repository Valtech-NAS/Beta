namespace SFA.Apprenticeships.Infrastructure.Postcode.IntegrationTests
{
    using System;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using RestSharp;
    using SFA.Apprenticeships.Infrastructure.Postcode;
    using SFA.Apprenticeships.Infrastructure.Postcode.Entities;

    [TestFixture]
    public class PostcodeServiceTests
    {
        private const string PostcodeIOUrl = "http://api.postcodes.io";
        private const string SfaPostCode = "CV1 2WT";

        [TestCase(null)]
        public void DoesConstructorThrowExceptionWithNullUrl(string url)
        {
            Action test = () => new PostcodeService(url);

            test.ShouldThrow<ArgumentNullException>();
        }

        [TestCase("")]
        public void DoesConstructorThrowExceptionWithNoUrl(string url)
        {
            Action test = () => new PostcodeService(url);

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void CanGetDefaultRestClientWithBaseUrl()
        {
            var service = new PostcodeService(PostcodeIOUrl);

            service.Client.BaseUrl.Should().Be(PostcodeIOUrl);
        }

        [Test]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            var service = new PostcodeService(PostcodeIOUrl);
            var location = service.GetLocation(SfaPostCode);
            location.GeoPoint.Latitude.Should().Be(52.4009991288043);
            location.GeoPoint.Longitude.Should().Be(-1.50812239495425);
        }

        [Test]
        public void ShouldReturnNullForNonExistentPostcode()
        {
            var service = new PostcodeService(PostcodeIOUrl);
            var location = service.GetLocation("ZZ1 0ZZ");
            location.Should().BeNull();
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

            var service = new PostcodeService(PostcodeIOUrl) { Client = restClient.Object };

            Action test = () => service.GetLocation(SfaPostCode);

            test.ShouldThrow<ApplicationException>();
        }
    }
}
