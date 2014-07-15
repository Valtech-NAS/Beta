namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Common.IoC;
    using Configuration;
    using EasyNetQ;
    using EasyNetQ.Management.Client;
    using EasyNetQ.Management.Client.Model;
    using FluentAssertions;
    using Interfaces;
    using IoC;
    using NUnit.Framework;
    using StructureMap;
    using Tests.Consumers;

    [TestFixture]
    public class BootstrapSubscribersTests
    {
        private const string ExchangeName =
            "SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers.TestMessage:SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests";

        private const string QueueNameSync =
            "SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers.TestMessage:SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests_TestMessageConsumerSync";

        private const string QueueNameAsync =
            "SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers.TestMessage:SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests_TestMessageConsumerAsync";

        private IManagementClient _managementClient;

        [TestFixtureSetUp]
        public void BeforeAllTests()
        {
            IRabbitMqHostConfiguration rabitConfig = RabbitMqHostsConfiguration.Instance.RabbitHosts["Test"];
            _managementClient = new ManagementClient(string.Format("http://{0}", rabitConfig.HostName),
                rabitConfig.UserName, rabitConfig.Password);

            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
            });


            var bs = ObjectFactory.GetInstance<IBootstrapSubcribers>();
            bs.LoadSubscribers(Assembly.GetExecutingAssembly(), "test_app");
        }

        [TestFixtureTearDown]
        public void AfterAllTests()
        {
            Queue notAsyncQueue = GetQueue(QueueNameSync);
            if (notAsyncQueue != null)
            {
                _managementClient.DeleteQueue(notAsyncQueue);
            }

            Queue asyncQueue = GetQueue(QueueNameAsync);
            if (asyncQueue != null)
            {
                _managementClient.DeleteQueue(asyncQueue);
            }

            Exchange exchange = GetExchange(ExchangeName);
            if (exchange != null)
            {
                _managementClient.DeleteExchange(exchange);
            }

            // Needed or tests were hanging.
            var bus = ObjectFactory.GetInstance<IBus>();
            bus.Advanced.Dispose();
        }

        protected Queue GetQueue(string queueName)
        {
            IEnumerable<Queue> queues = _managementClient.GetQueues();
            Queue queue = queues.SingleOrDefault(q => q.Name == queueName);
            return queue;
        }

        protected Exchange GetExchange(string exchangeName)
        {
            IEnumerable<Exchange> exchanges = _managementClient.GetExchanges();
            Exchange exchange = exchanges.SingleOrDefault(q => q.Name == exchangeName);
            return exchange;
        }

        [TestCase]
        public void AutoBindsSubscriptions()
        {
            Exchange exchange = GetExchange(ExchangeName);
            exchange.Should().NotBeNull();
            exchange.Durable.Should().Be(true);

            Queue syncQueue = GetQueue(QueueNameSync);
            syncQueue.Should().NotBeNull();
            syncQueue.Durable.Should().Be(true);
            syncQueue.AutoDelete.Should().Be(false);

            IEnumerable<Binding> syncBindings = _managementClient.GetBindingsForQueue(syncQueue);
            syncBindings.Count().Should().Be(2);
            Binding syncBindingToSelf = syncBindings.SingleOrDefault(b => b.RoutingKey == QueueNameSync);
            syncBindingToSelf.Should().NotBeNull();
            Binding syncBindingToExchange = syncBindings.SingleOrDefault(b => b.RoutingKey == "#");
            syncBindingToExchange.Should().NotBeNull();

            Queue asyncQueue = GetQueue(QueueNameAsync);
            asyncQueue.Should().NotBeNull();
            asyncQueue.Durable.Should().Be(true);
            asyncQueue.AutoDelete.Should().Be(false);

            IEnumerable<Binding> asyncBindings = _managementClient.GetBindingsForQueue(asyncQueue);
            asyncBindings.Count().Should().Be(2);
            Binding asyncBindingToSelf = asyncBindings.SingleOrDefault(b => b.RoutingKey == QueueNameAsync);
            asyncBindingToSelf.Should().NotBeNull();
            Binding asyncBindingToExchange = asyncBindings.SingleOrDefault(b => b.RoutingKey == "#");
            asyncBindingToExchange.Should().NotBeNull();
        }

        [Test]
        public void ConsumesSyncAndAsyncMessagesFromQueue()
        {
            var bus = ObjectFactory.GetInstance<IBus>();
            var testMessage = new TestMessage {TestString = "Testing 123"};

            bus.Publish(testMessage);
            Thread.Sleep(3000);

            // Is 2 becasue both the sync and async subscribers received the same message
            ConsumerCounter.Counter.Should().Be(2);
        }
    }
}