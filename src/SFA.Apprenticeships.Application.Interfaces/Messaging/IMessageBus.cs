namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    //todo: should move to domain interfaces?
    public interface IMessageBus
    {
        void PublishMessage<T>(T message) where T : class;
    }
}
