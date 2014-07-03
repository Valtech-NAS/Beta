namespace SFA.Apprenticeships.Infrastructure.Postcode.IntegrationTests
{
    using System;
    using Application.Interfaces.Locations;
    using Common.IoC;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class PostcodeServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }

        [Test]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            var service = ObjectFactory.GetInstance<IPostcodeLookupProvider>();
            var location = service.GetLocation("CV1 2WT");
            location.GeoPoint.Latitude.Should().Be(52.4009991288043);
            location.GeoPoint.Longitude.Should().Be(-1.50812239495425);
        }

        [Test]
        public void ShouldReturnNullForNonExistentPostcode()
        {
            var service = ObjectFactory.GetInstance<IPostcodeLookupProvider>();
            var location = service.GetLocation("ZZ1 0ZZ");
            location.Should().BeNull();
        }
    }
}
