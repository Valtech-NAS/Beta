namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using StructureMap;

    public class StructureMapMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IContainer _container;

        public StructureMapMessageDispatcher(IContainer container)
        {
            _container = container;
        }

        public void Dispatch<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : IConsume<TMessage>
        {
            var consumer = _container.GetInstance<TConsumer>();
            consumer.Consume(message);
        }

        public Task DispatchAsync<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : IConsumeAsync<TMessage>
        {
            var consumer = _container.GetInstance<TConsumer>();
            return consumer.Consume(message);
        }
    }
}
