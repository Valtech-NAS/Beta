namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using System;
    using System.Reflection;
    using System.Threading;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers;

    [TestFixture]
    public class PublishSubscribeTests : RabbitSetUp
    {
        [Test]
        public void CanConsumeMessages()
        {
            var bus = Transport.CreateBus();
            var bs = new Bootstrapper(bus);
            bs.LoadConsumers(Assembly.GetExecutingAssembly(), "test_app");

            var testMessage = new TestMessage() { TestString = "Testing 123" };

            bus.Publish(testMessage);
            Thread.Sleep(2000);
            ConsumerCounter.Counter.Should().Be(2);
        }
    }
}
