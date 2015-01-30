namespace SFA.Apprenticeships.Infrastructure.Postcode.IntegrationTests
{
    using Application.Location;
    using Common.IoC;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class PostcodeServiceTests
    {
        private Container _container;
        [SetUp]
        public void SetUp()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }

        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            var service = _container.GetInstance<IPostcodeLookupProvider>();

            var location = service.GetLocation("CV1 2WT");
            location.GeoPoint.Latitude.Should().Be(52.4009991288043);
            location.GeoPoint.Longitude.Should().Be(-1.50812239495425);
        }

        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPartialPostcode()
        {
            var service = _container.GetInstance<IPostcodeLookupProvider>();

            var location = service.GetLocation("CV1");
            location.GeoPoint.Latitude.Should().Be(52.4084714696457);
            location.GeoPoint.Longitude.Should().Be(-1.50508839395149);
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForNonExistentPostcode()
        {
            var service = _container.GetInstance<IPostcodeLookupProvider>();

            var location = service.GetLocation("ZZ1 0ZZ");
            location.Should().BeNull();
        }
    }
}
