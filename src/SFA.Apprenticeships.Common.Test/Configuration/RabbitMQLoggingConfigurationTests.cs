namespace SFA.Apprenticeships.Common.Caching.Tests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Logging.Configuration;

    [TestFixture]
    public class RabbitMQLoggingConfigurationTests
    {
        [Test]
        public void PopulateConfgurationCorrectly()
        {
            var testconfig = RabbitMQLoggingConfigurationSection.ConfigurationSectionDetails;

            testconfig.VirtualHost.Should().Be("testvh");
            testconfig.UserName.Should().Be("testun");
            testconfig.Password.Should().Be("testpw");
            testconfig.Port.Should().Be(1234);
            testconfig.RoutingKey.Should().Be("testroutingkey");
            testconfig.HostName.Should().Be("testhn");
            testconfig.ExchangeName.Should().Be("testen");
            testconfig.ExchangeType.Should().Be("Fanout");
            testconfig.Durable.Should().Be(false);
            testconfig.AppId.Should().Be("testappid");
            testconfig.HeartBeatSeconds.Should().Be(4321);
            testconfig.QueueName.Should().Be("testqueuename");
            testconfig.OutputEasyNetQLogsToNLogInternal.Should().Be(true);
        }
    }
}
