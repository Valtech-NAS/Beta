namespace SFA.Apprenticeships.Common.Messaging
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using StructureMap;

    public class StructureMapMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IContainer container;

        public StructureMapMessageDispatcher(IContainer container)
        {
            this.container = container;
        }

        public void Dispatch<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : IConsume<TMessage>
        {
            var consumer = container.GetInstance<TConsumer>();
            consumer.Consume(message);
        }

        public Task DispatchAsync<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : IConsumeAsync<TMessage>
        {
            var consumer = container.GetInstance<TConsumer>();
            return consumer.Consume(message);
        }
    }
}
