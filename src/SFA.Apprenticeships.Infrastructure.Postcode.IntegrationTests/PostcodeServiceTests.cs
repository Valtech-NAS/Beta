namespace SFA.Apprenticeships.Infrastructure.Postcode.IntegrationTests
{
    using System;
    using Application.Location;
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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
#pragma warning restore 0618
        }

        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPostcode()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<IPostcodeLookupProvider>();
#pragma warning restore 0618

            var location = service.GetLocation("CV1 2WT");
            location.GeoPoint.Latitude.Should().Be(52.4009991288043);
            location.GeoPoint.Longitude.Should().Be(-1.50812239495425);
        }

        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPartialPostcode()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<IPostcodeLookupProvider>();
#pragma warning restore 0618

            var location = service.GetLocation("CV1");
            location.GeoPoint.Latitude.Should().Be(52.4084714696457);
            location.GeoPoint.Longitude.Should().Be(-1.50508839395149);
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForNonExistentPostcode()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<IPostcodeLookupProvider>();
#pragma warning restore 0618

            var location = service.GetLocation("ZZ1 0ZZ");
            location.Should().BeNull();
        }
    }
}
