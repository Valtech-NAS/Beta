namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;

    public class TestConsumerAsync : IConsumeAsync<TestMessage>
    {
        [AutoSubscriberConsumerAttribute(SubscriptionId = "TestMessageConsumerAsync")]
        public Task Consume(TestMessage message)
        {
            return Task.Run(() => ConsumeTask(message));
        }

        private void ConsumeTask(TestMessage message)
        {
            Console.WriteLine("TestMessageConsumerAsync recieved message with TestString:" + message.TestString);
            ConsumerCounter.IncrementCounter();
        }
    }
}
