namespace SFA.Apprenticeships.Common.Tests.Configuration
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration.LegacyServices;

    [TestFixture]
    public class LegacyServicesConfigurationTests
    {
        [Test]
        public void PopulateConfgurationCorrectly()
        {
            var testconfig = LegacyServicesConfiguration.Instance;

            testconfig.SystemId.Should().Be(Guid.Parse("9a9d2261-68c9-4be1-bfe5-0db189d5fdfe"));
            testconfig.PublicKey.Should().Be("this is the public key :)");
        }
    }
}
