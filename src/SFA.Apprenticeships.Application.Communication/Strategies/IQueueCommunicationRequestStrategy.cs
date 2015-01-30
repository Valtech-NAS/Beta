namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public interface IQueueCommunicationRequestStrategy
    {
        void Queue(Guid candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
