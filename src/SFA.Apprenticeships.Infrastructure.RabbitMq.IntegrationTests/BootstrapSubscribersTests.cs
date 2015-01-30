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

        private Container _container;

        [TestFixtureSetUp]
        public void BeforeAllTests()
        {
            var rabitConfig = RabbitMqHostsConfiguration.Instance.RabbitHosts["Messaging"]; //previously was test
            _managementClient = new ManagementClient(string.Format("http://{0}", rabitConfig.HostName),
                rabitConfig.UserName, rabitConfig.Password);

            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
            });

            var bs = _container.GetInstance<IBootstrapSubcribers>();

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
            var bus = _container.GetInstance<IBus>();

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

        [Test, Category("Integration")]
        public void ConsumesSyncAndAsyncMessagesFromQueue()
        {
            var bus = _container.GetInstance<IBus>();

            var testMessage = new TestMessage { TestString = "Testing 123" };

            bus.Publish(testMessage);
            Thread.Sleep(5000);

            // Is 2 becasue both the sync and async subscribers received the same message
            ConsumerCounter.Counter.Should().Be(2);
        }
    }
}