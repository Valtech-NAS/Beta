namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests.RabbitMQ
{
    using System.Reflection;
    using System.Threading;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers;
    using SFA.Apprenticeships.Common.Messaging.RabbitMQ;

    [TestFixture]
    public class PublishSubscribeTests : RabbitSetUp
    {
        [Test]
        public void ConsumesSyncAndAsyncMessagesFromQueue()
        {
            var bus = Transport.CreateBus();
            var bs = new BootstrapSubcribers(bus);
            bs.LoadSubscribers(Assembly.GetExecutingAssembly(), "test_app");

            var testMessage = new TestMessage() { TestString = "Testing 123" };

            bus.Publish(testMessage);
            Thread.Sleep(2000);
            ConsumerCounter.Counter.Should().Be(2);
        }
    }
}
