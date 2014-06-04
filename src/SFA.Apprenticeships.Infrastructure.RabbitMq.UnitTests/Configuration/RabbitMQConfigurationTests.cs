namespace SFA.Apprenticeships.Infrastructure.RabbitMq.UnitTests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;

    [TestFixture]
    public class RabbitMqConfigurationTests
    {
        /// <summary>
        /// Test config laods and reads all values
        /// Do not move to integration tests and would need to check against
        /// real config values which would be a security concern.
        /// </summary>
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
            testconfig1.PreFetchCount.Should().Be(1);

            var testconfig2 = RabbitMqHostsConfiguration.Instance.RabbitHosts["Two"];

            testconfig2.VirtualHost.Should().Be("testvh2");
            testconfig2.UserName.Should().Be("testun2");
            testconfig2.Password.Should().Be("testpw2");
            testconfig2.Port.Should().Be(1235);
            testconfig2.HostName.Should().Be("testhn2");
            testconfig2.Durable.Should().Be(true);
            testconfig2.HeartBeatSeconds.Should().Be(4322);
            testconfig2.OutputEasyNetQLogsToNLogInternal.Should().Be(false);
            testconfig1.PreFetchCount.Should().Be(1);
        }
    }
}
