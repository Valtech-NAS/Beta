namespace SFA.Apprenticeships.Infrastructure.RabbitMq
{
    using Domain.Interfaces.Messaging;
    using EasyNetQ;

    public class RabbitMessageBus : IMessageBus
    {
        private readonly IBus _bus;

        public RabbitMessageBus(IBus bus)
        {
            _bus = bus;
        }

        public void PublishMessage<T>(T message) where T : class
        {
            _bus.Publish(message);
        }
    }
}
