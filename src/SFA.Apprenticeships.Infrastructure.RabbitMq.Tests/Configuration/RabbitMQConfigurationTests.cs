namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;

    [TestFixture]
    public class RabbitMqConfigurationTests
    {
        [Test]
        public void PopulateConfgurationCorrectly()
        {
            var rabbitHostsConfig = RabbitMqHostsConfiguration.Instance;

            rabbitHostsConfig.DefaultHost.Should().Be("Test");
            
            var testconfig1 = rabbitHostsConfig.RabbitHosts["Test"];

            //Conatins real config so don't test these values.
            testconfig1.VirtualHost.Should().NotBeEmpty();
            testconfig1.UserName.Should().NotBeEmpty();
            testconfig1.Password.Should().NotBeEmpty();
            testconfig1.Port.Should().BeGreaterThan(0);
            testconfig1.HostName.Should().NotBeEmpty();
            testconfig1.Durable.Should().Be(true);
            testconfig1.HeartBeatSeconds.Should().BeGreaterThan(0);
            testconfig1.OutputEasyNetQLogsToNLogInternal.Should().Be(false);
            testconfig1.PreFetchCount.Should().Be(1);

            var testconfig2 = RabbitMqHostsConfiguration.Instance.RabbitHosts["Two"];

            testconfig2.VirtualHost.Should().Be("testvh2");
            testconfig2.UserName.Should().Be("testun2");
            testconfig2.Password.Should().Be("testpw2");
            testconfig2.Port.Should().Be(1235);
            testconfig2.HostName.Should().Be("testhn2");
            testconfig2.Durable.Should().Be(false);
            testconfig2.HeartBeatSeconds.Should().Be(4322);
            testconfig2.OutputEasyNetQLogsToNLogInternal.Should().Be(true);
            testconfig1.PreFetchCount.Should().Be(1);
        }
    }
}
