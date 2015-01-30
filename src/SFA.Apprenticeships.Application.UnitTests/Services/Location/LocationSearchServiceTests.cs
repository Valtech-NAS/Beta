namespace SFA.Apprenticeships.Application.UnitTests.Services.Location
{
    using Moq;
    using NUnit.Framework;
    using Application.Location;

    [TestFixture]
    public class LocationSearchServiceTests
    {
        private const string ValidCompletePostcode = "CV1 2WT";
        private const string ValidPartialPostcode = "CV1";
        private const string ALocation = "Coventry";
        private readonly Mock<ILocationLookupProvider> _locationLookupProvider = new Mock<ILocationLookupProvider>();
        private readonly Mock<IPostcodeLookupProvider> _postcodeLookupProvider = new Mock<IPostcodeLookupProvider>();

        [Test]
        public void ShouldCallToPostcodeProviderIfTheInputIsAPostcode()
        {
            var locationLookupService = new LocationSearchService(_locationLookupProvider.Object,
                _postcodeLookupProvider.Object);

            locationLookupService.FindLocation(ValidCompletePostcode);

            _postcodeLookupProvider.Verify(plp => plp.GetLocation(ValidCompletePostcode));
        }

        [Test]
        public void ShouldCallToPostcodeProviderIfTheInputIsAPartialPostcode()
        {
            var locationLookupService = new LocationSearchService(_locationLookupProvider.Object,
                _postcodeLookupProvider.Object);

            locationLookupService.FindLocation(ValidPartialPostcode);

            _postcodeLookupProvider.Verify(plp => plp.GetLocation(ValidPartialPostcode));
        }

        [Test]
        public void ShouldCallToLocationProviderIfTheInputIsALocation()
        {
            var locationLookupService = new LocationSearchService(_locationLookupProvider.Object,
                _postcodeLookupProvider.Object);

            locationLookupService.FindLocation(ALocation);

            _locationLookupProvider.Verify(llp => llp.FindLocation(ALocation, 50));
        }
    }
}