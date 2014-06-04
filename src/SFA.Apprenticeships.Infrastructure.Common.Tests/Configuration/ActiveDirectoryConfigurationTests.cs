namespace SFA.Apprenticeships.Infrastructure.Common.Tests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Common.ActiveDirectory;

    [TestFixture]
    public class ActiveDirectoryConfigurationTests
    {
        [TestCase]
        public void GetConfigSectionForAd()
        {
            var test = ActiveDirectoryConfiguration.Instance;
           
            test.DistinguishedName.Should().Be("distinguishedname");
            test.Server.Should().Be("server");
            test.Username.Should().Be("username");
            test.Password.Should().Be("password");
        }
    }
}
