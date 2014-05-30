namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    public interface IMessageBus
    {
        void PublishMessage<T>(T message) where T : class;
    }
}
