namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    using System;

    public interface IMessageBus
    {
        void PublishMessage<T>(T message) where T : class;
    }
}
