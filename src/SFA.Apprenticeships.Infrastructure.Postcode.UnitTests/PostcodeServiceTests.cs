namespace SFA.Apprenticeships.Infrastructure.Postcode.UnitTests
{
    using Moq;
    using NUnit.Framework;
    using RestSharp;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Infrastructure.Postcode;
    using SFA.Apprenticeships.Infrastructure.Postcode.Entities;

    [TestFixture]
    public class PostcodeServiceTests
    {
        private Mock<PostcodeService> _postcodeService;

        [SetUp]
        public void SetUp()
        {
            var configurationManager = new Mock<IConfigurationManager>();
            configurationManager.Setup(cm => cm.GetAppSetting("PostcodeServiceEndpoint"))
                .Returns("http://api.postodes.io");
            _postcodeService = new Mock<PostcodeService>(MockBehavior.Loose, configurationManager.Object) { CallBase = true };
            _postcodeService.Setup(ps => ps.Execute<PostcodeInfoResult>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<PostcodeInfoResult>());
        }

        [Test]
        public void ShouldCallToPostcodeUrlIfItsACompletePostcode()
        {
            _postcodeService.Object.GetLocation("CV1 2WT");

            _postcodeService
                .Verify(ps => ps.Execute<PostcodeInfoResult>(It.Is<IRestRequest>(r => r.Resource.StartsWith("postcodes"))));
        }

        [Test]
        public void ShouldCallToOutcodeUrlIfItsAPartialPostcode()
        {
            _postcodeService.Object.GetLocation("CV1");

            _postcodeService
                .Verify(ps => ps.Execute<PostcodeInfoResult>(It.Is<IRestRequest>(r => r.Resource.StartsWith("outcodes"))));
        }
    }
}