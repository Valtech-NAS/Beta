namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers
{
    using System;
    using EasyNetQ.AutoSubscribe;

    public class TestConsumerSync : IConsume<TestMessage>
    {
        [AutoSubscriberConsumerAttribute(SubscriptionId = "TestMessageConsumerSync")]
        public void Consume(TestMessage message)
        {
            Console.WriteLine("TestMessageConsumerSync recieved message with TestString:" + message.TestString);
            ConsumerCounter.IncrementCounter();
        }
    }
}
