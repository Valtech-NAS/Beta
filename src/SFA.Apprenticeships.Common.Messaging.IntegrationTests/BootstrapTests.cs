namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using EasyNetQ.Management.Client;
    using EasyNetQ.Management.Client.Model;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration.Messaging;

    [TestFixture]
    class BootstrapTests
    {
        private const string ExchangeName = "SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers.TestMessage:SFA.Apprenticeships.Common.Messaging.IntegrationTests";
        private const string QueueNamre_Sync = "SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers.TestMessage:SFA.Apprenticeships.Common.Messaging.IntegrationTests_TestMessageConsumerSync";
        private const string QueueNamre_Async = "SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers.TestMessage:SFA.Apprenticeships.Common.Messaging.IntegrationTests_TestMessageConsumerAsync";

        private IManagementClient managementClient;

        [SetUp]
        public void Setup()
        {
            var rabitConfig = RabbitMQConfigurationSection.Instance;
            managementClient = new ManagementClient(string.Format("http://{0}", rabitConfig.HostName), rabitConfig.UserName, rabitConfig.Password);
            TearDown();
        }

        [TearDown]
        public void TearDown()
        {
            var notAsyncQueue = GetQueue(QueueNamre_Sync);
            if (notAsyncQueue != null)
            {
                managementClient.DeleteQueue(notAsyncQueue);
            }

            var asyncQueue = GetQueue(QueueNamre_Async);
            if (asyncQueue != null)
            {
                managementClient.DeleteQueue(asyncQueue);
            }

            var exchange = GetExchange(ExchangeName);
            if (exchange != null)
            {
                managementClient.DeleteExchange(exchange);
            }
        }

        [Test]
        public void AutoBindBindsSubscriptions()
        {
            var bus = Transport.CreateBus();
            var bs = new Bootstrapper(bus);
            bs.LoadConsumers(Assembly.GetExecutingAssembly(), "test_app");

            var exchange = GetExchange(ExchangeName);
            exchange.Should().NotBeNull();
            exchange.Durable.Should().Be(true);

            var syncQueue = GetQueue(QueueNamre_Sync);
            syncQueue.Should().NotBeNull();
            syncQueue.Durable.Should().Be(true);
            syncQueue.AutoDelete.Should().Be(false);

            var syncBindings = managementClient.GetBindingsForQueue(syncQueue);
            syncBindings.Count().Should().Be(2);
            var syncBindingToSelf = syncBindings.SingleOrDefault(b => b.RoutingKey == QueueNamre_Sync);
            syncBindingToSelf.Should().NotBeNull();
            var syncBindingToExchange = syncBindings.SingleOrDefault(b => b.RoutingKey == "#");
            syncBindingToExchange.Should().NotBeNull();

            var asyncQueue = GetQueue(QueueNamre_Async);
            asyncQueue.Should().NotBeNull();
            asyncQueue.Durable.Should().Be(true);
            asyncQueue.AutoDelete.Should().Be(false);

            var asyncBindings = managementClient.GetBindingsForQueue(asyncQueue);
            asyncBindings.Count().Should().Be(2);
            var asyncBindingToSelf = asyncBindings.SingleOrDefault(b => b.RoutingKey == QueueNamre_Async);
            asyncBindingToSelf.Should().NotBeNull();
            var asyncBindingToExchange = asyncBindings.SingleOrDefault(b => b.RoutingKey == "#");
            asyncBindingToExchange.Should().NotBeNull();
        }

        private Queue GetQueue(string queueName)
        {
            var queues = managementClient.GetQueues();
            var queue = queues.SingleOrDefault(q => q.Name == queueName);
            return queue;
        }

        private Exchange GetExchange(string exchangeName)
        {
            var exchanges = managementClient.GetExchanges();
            var exchange = exchanges.SingleOrDefault(q => q.Name == exchangeName);
            return exchange;
        }
    }
}
