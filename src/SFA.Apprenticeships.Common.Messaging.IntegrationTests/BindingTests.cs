namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using System;
    using System.Reflection;
    using System.Threading;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers;

    [TestFixture]
    public class BindingTests
    {
        //[Test]
        //public void CanConsumeMessage()
        //{
        //    var bus = Transport.CreateBus();
        //    var bs = new Bootstrapper(bus);
        //    bs.LoadConsumers(Assembly.GetExecutingAssembly(), "test_app");

        //    var testMessage = new TestMessage() { TestString = "Testing 123" }
        //    var currentCounter = TestMessage.Counter;

        //    bus.Publish(testMessage);

        //    Thread.Sleep(5000);

        //    currentCounter.Should().Be(TestMessage.Counter - 1);
        //}

        [Test]
        public void CanConsumeAsyncMessage()
        {

        }

        [Test]
        public void CanConsumeTopicMessage()
        {

        }

        [Test]
        public void CanConsumeTopicAsyncMessage()
        {

        }
    }
}
