namespace SFA.Apprenticeships.Infrastructure.UserDirectory.UnitTests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using UserDirectory.Configuration;

    [TestFixture]
    public class ActiveDirectoryConfigurationTests
    {
        [Test]
        public void GetConfigSectionForAd()
        {
            var test = ActiveDirectoryConfiguration.Instance;

            test.DistinguishedName.Should().Be("distinguishedname");
            test.Server.Should().Be("server");
            test.Username.Should().Be("username");
            test.Password.Should().Be("password");
            test.SecureMode.Should().BeTrue();
        }
    }
}
