namespace SFA.Apprenticeships.Infrastructure.RabbitMq
{
    using EasyNetQ;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;

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
