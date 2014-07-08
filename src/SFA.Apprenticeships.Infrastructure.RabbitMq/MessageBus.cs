namespace SFA.Apprenticeships.Infrastructure.RabbitMq
{
    using Domain.Interfaces.Messaging;
    using EasyNetQ;

    public class MessageBus : IMessageBus
    {
        private readonly IBus _bus;

        public MessageBus(IBus bus)
        {
            _bus = bus;
        }

        public void PublishMessage<T>(T message) where T : class
        {
            _bus.Publish(message);
        }
    }
}
