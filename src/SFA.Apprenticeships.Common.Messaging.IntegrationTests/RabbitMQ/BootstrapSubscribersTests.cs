namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests.RabbitMQ
{
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Messaging.Interfaces;
    using StructureMap;

    [TestFixture]
    public class BootstrapSubscribersTests : RabbitSetUp
    {
        [TestCase]
        public void AutoBindsSubscriptions()
        {
            var bs = ObjectFactory.GetInstance<IBootstrapSubcribers>();
            bs.LoadSubscribers(Assembly.GetExecutingAssembly(), "test_app");

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
    }
}
