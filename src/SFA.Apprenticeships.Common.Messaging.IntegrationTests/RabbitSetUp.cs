namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests
{
    using System.Linq;
    using EasyNetQ.Management.Client;
    using EasyNetQ.Management.Client.Model;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration.Messaging;

    [SetUpFixture]
    public class RabbitSetUp
    {
        protected const string ExchangeName = "SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers.TestMessage:SFA.Apprenticeships.Common.Messaging.IntegrationTests";
        protected const string QueueNamre_Sync = "SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers.TestMessage:SFA.Apprenticeships.Common.Messaging.IntegrationTests_TestMessageConsumerSync";
        protected const string QueueNamre_Async = "SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers.TestMessage:SFA.Apprenticeships.Common.Messaging.IntegrationTests_TestMessageConsumerAsync";

        protected IManagementClient managementClient;

        [SetUp]
        public virtual void BeforeAllTests()
        {
            Common.IoC.IoC.Initialize();
            var rabitConfig = RabbitMQConfigurationSection.Instance;
            managementClient = new ManagementClient(string.Format("http://{0}", rabitConfig.HostName), rabitConfig.UserName, rabitConfig.Password);
        }

        [TearDown]
        public virtual void AfterAllTests()
        {
            var notAsyncQueue = GetQueue(QueueNamre_Sync);
            if (notAsyncQueue != null)
            {
                managementClient.DeleteQueue(notAsyncQueue);
            }

            var asyncQueue = GetQueue(QueueNamre_Async);
            if (asyncQueue != null)
            {
                managementClient.DeleteQueue(asyncQueue);
            }

            var exchange = GetExchange(ExchangeName);
            if (exchange != null)
            {
                managementClient.DeleteExchange(exchange);
            }
        }
        protected Queue GetQueue(string queueName)
        {
            var queues = managementClient.GetQueues();
            var queue = queues.SingleOrDefault(q => q.Name == queueName);
            return queue;
        }

        protected Exchange GetExchange(string exchangeName)
        {
            var exchanges = managementClient.GetExchanges();
            var exchange = exchanges.SingleOrDefault(q => q.Name == exchangeName);
            return exchange;
        }
    }
}
