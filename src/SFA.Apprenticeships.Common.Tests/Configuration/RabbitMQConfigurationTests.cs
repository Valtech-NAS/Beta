namespace SFA.Apprenticeships.Common.Tests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class RabbitMqConfigurationTests
    {
        [Test]
        public void PopulateConfgurationCorrectly()
        {
            var rabbitHostsConfig = RabbitMqHostsConfiguration.Instance;

            rabbitHostsConfig.DefaultHost.Should().Be("One");
            
            var testconfig1 = rabbitHostsConfig.RabbitHosts["One"];

            testconfig1.VirtualHost.Should().Be("testvh1");
            testconfig1.UserName.Should().Be("testun1");
            testconfig1.Password.Should().Be("testpw1");
            testconfig1.Port.Should().Be(1234);
            testconfig1.HostName.Should().Be("testhn1");
            testconfig1.Durable.Should().Be(false);
            testconfig1.HeartBeatSeconds.Should().Be(4321);
            testconfig1.OutputEasyNetQLogsToNLogInternal.Should().Be(true);

            var testconfig2 = RabbitMqHostsConfiguration.Instance.RabbitHosts["Two"];

            testconfig2.VirtualHost.Should().Be("testvh2");
            testconfig2.UserName.Should().Be("testun2");
            testconfig2.Password.Should().Be("testpw2");
            testconfig2.Port.Should().Be(1235);
            testconfig2.HostName.Should().Be("testhn2");
            testconfig2.Durable.Should().Be(true);
            testconfig2.HeartBeatSeconds.Should().Be(4322);
            testconfig2.OutputEasyNetQLogsToNLogInternal.Should().Be(false);
        }
    }
}
