namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Messaging;

    public interface ISendCandidateCommunicationStrategy
    {
        void Send(Guid candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
