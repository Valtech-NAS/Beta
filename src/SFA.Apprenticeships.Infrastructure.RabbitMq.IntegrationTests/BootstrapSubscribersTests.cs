namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests
{
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
            var rabitConfig = RabbitMqHostsConfiguration.Instance.RabbitHosts["Messaging"]; //previously was test
            _managementClient = new ManagementClient(string.Format("http://{0}", rabitConfig.HostName),
                rabitConfig.UserName, rabitConfig.Password);

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
            });

            var bs = ObjectFactory.GetInstance<IBootstrapSubcribers>();
#pragma warning restore 0618

            bs.LoadSubscribers(Assembly.GetExecutingAssembly(), "VacancyEtl"); //previously was test_app
        }

        [TestFixtureTearDown]
        public void AfterAllTests()
        {
            var notAsyncQueue = GetQueue(QueueNameSync);
            if (notAsyncQueue != null)
            {
                _managementClient.DeleteQueue(notAsyncQueue);
            }

            var asyncQueue = GetQueue(QueueNameAsync);
            if (asyncQueue != null)
            {
                _managementClient.DeleteQueue(asyncQueue);
            }

            var exchange = GetExchange(ExchangeName);
            if (exchange != null)
            {
                _managementClient.DeleteExchange(exchange);
            }

            // Needed or tests were hanging.
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var bus = ObjectFactory.GetInstance<IBus>();
#pragma warning restore 0618

            bus.Advanced.Dispose();
        }

        protected Queue GetQueue(string queueName)
        {
            var queues = _managementClient.GetQueues();
            var queue = queues.SingleOrDefault(q => q.Name == queueName);
            return queue;
        }

        protected Exchange GetExchange(string exchangeName)
        {
            var exchanges = _managementClient.GetExchanges();
            var exchange = exchanges.SingleOrDefault(q => q.Name == exchangeName);
            return exchange;
        }

        [TestCase, Category("Integration"), Ignore("Has this test value?")]
        public void AutoBindsSubscriptions()
        {
            var exchange = GetExchange(ExchangeName);
            exchange.Should().NotBeNull();
            exchange.Durable.Should().Be(true);

            var syncQueue = GetQueue(QueueNameSync);
            syncQueue.Should().NotBeNull();
            syncQueue.Durable.Should().Be(true);
            syncQueue.AutoDelete.Should().Be(false);

            var syncBindings = _managementClient.GetBindingsForQueue(syncQueue);
            syncBindings.Count().Should().Be(2);
            var syncBindingToSelf = syncBindings.SingleOrDefault(b => b.RoutingKey == QueueNameSync);
            syncBindingToSelf.Should().NotBeNull();
            var syncBindingToExchange = syncBindings.SingleOrDefault(b => b.RoutingKey == "#");
            syncBindingToExchange.Should().NotBeNull();

            var asyncQueue = GetQueue(QueueNameAsync);
            asyncQueue.Should().NotBeNull();
            asyncQueue.Durable.Should().Be(true);
            asyncQueue.AutoDelete.Should().Be(false);

            var asyncBindings = _managementClient.GetBindingsForQueue(asyncQueue);
            asyncBindings.Count().Should().Be(4);
            var asyncBindingToSelf = asyncBindings.SingleOrDefault(b => b.RoutingKey == QueueNameAsync);
            asyncBindingToSelf.Should().NotBeNull();
            var asyncBindingToExchange = asyncBindings.SingleOrDefault(b => b.RoutingKey == "#");
            asyncBindingToExchange.Should().NotBeNull();
        }

        [Test, Category("Integration")]
        public void ConsumesSyncAndAsyncMessagesFromQueue()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var bus = ObjectFactory.GetInstance<IBus>();
#pragma warning restore 0618

            var testMessage = new TestMessage { TestString = "Testing 123" };

            bus.Publish(testMessage);
            Thread.Sleep(5000);

            // Is 2 becasue both the sync and async subscribers received the same message
            ConsumerCounter.Counter.Should().Be(2);
        }
    }
}