﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.UnitTests.Configuration
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using LegacyWebServices.Configuration;

    [TestFixture]
    public class LegacyServicesConfigurationTests
    {
        [Test]
        public void PopulateConfgurationCorrectly()
        {
            var testconfig = LegacyServicesConfiguration.Instance;

            testconfig.SystemId.Should().Be(Guid.Parse("9a9d226168c94be1bfe50db189d5fdfe"));
            testconfig.PublicKey.Should().Be("this is the public key :)");
        }
    }
}