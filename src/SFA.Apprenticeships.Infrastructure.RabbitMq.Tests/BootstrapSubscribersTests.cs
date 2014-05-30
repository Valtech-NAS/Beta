namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.RabbitMQ
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using EasyNetQ;
    using EasyNetQ.Management.Client;
    using EasyNetQ.Management.Client.Model;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.IoC;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers;
    using StructureMap;

    [TestFixture]
    public class BootstrapSubscribersTests 
    {
        private const string ExchangeName = "SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers.TestMessage:SFA.Apprenticeships.Infrastructure.RabbitMq.Tests";
        private const string QueueNamre_Sync = "SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers.TestMessage:SFA.Apprenticeships.Infrastructure.RabbitMq.Tests_TestMessageConsumerSync";
        private const string QueueNamre_Async = "SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers.TestMessage:SFA.Apprenticeships.Infrastructure.RabbitMq.Tests_TestMessageConsumerAsync";

        private IManagementClient managementClient;

        [TestFixtureSetUp]
        public void BeforeAllTests()
        {
            var rabitConfig = RabbitMqHostsConfiguration.Instance.RabbitHosts["Test"];
            managementClient = new ManagementClient(string.Format("http://{0}", rabitConfig.HostName), rabitConfig.UserName, rabitConfig.Password);

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

            // Needed or tests were hanging.
            var bus = ObjectFactory.GetInstance<IBus>();
            bus.Advanced.Dispose();
        }
        protected Queue GetQueue(string queueName)
        {
            var queues = managementClient.GetQueues();
            var queue = queues.SingleOrDefault(q => q.Name == queueName);
            return queue;
        }

        protected Exchange GetExchange(string exchangeName)
        {
            var exchanges = managementClient.GetExchanges();
            var exchange = exchanges.SingleOrDefault(q => q.Name == exchangeName);
            return exchange;
        }

        [TestCase]
        public void AutoBindsSubscriptions()
        {
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

        [Test]
        public void ConsumesSyncAndAsyncMessagesFromQueue()
        {
            var bus = ObjectFactory.GetInstance<IBus>();
            var testMessage = new TestMessage() { TestString = "Testing 123" };

            bus.Publish(testMessage);
            Thread.Sleep(3000);
            
            // Is 2 becasue both the sync and async subscribers received the same message
            ConsumerCounter.Counter.Should().Be(2);
        }
    }
}
