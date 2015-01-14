namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;

    public interface IQueueCommunicationRequestStrategy
    {
        void Queue(Guid candidateId, MessageTypes messageType, IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}