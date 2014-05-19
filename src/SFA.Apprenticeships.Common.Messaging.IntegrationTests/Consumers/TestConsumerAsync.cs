﻿namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers
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
            Console.WriteLine(message.TestString);
        }
    }
}
