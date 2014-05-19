namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;

    public class TestConsumerSync : IConsume<TestMessage>
    {
        [AutoSubscriberConsumerAttribute(SubscriptionId = "TestMessageConsumerSync")]
        public void Consume(TestMessage message)
        {
            Console.WriteLine(message.TestString);
        }
    }
}
