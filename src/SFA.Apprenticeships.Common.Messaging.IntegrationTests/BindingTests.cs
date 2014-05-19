namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using System.Reflection;
    using System.Threading;
    using NUnit.Framework;

    [TestFixture]
    public class BindingTests
    {
        [Test]
        public void CanConsumeMessage()
        {
            //var bus = Transport.CreateBus();
            //var bs = new Bootstrapper(bus);
            //bs.LoadConsumers(Assembly.GetExecutingAssembly(), "test_app");

            //bus.Publish(new TestMessage(){Message = "Testing 123"});

            Thread.Sleep(1000);
        }

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

        public class TestMessage
        {
            public string Message { get; set; }
        }
    }
}
