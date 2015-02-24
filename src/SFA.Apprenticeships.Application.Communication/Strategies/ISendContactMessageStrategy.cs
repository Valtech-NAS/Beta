namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Communications;

    public interface ISendContactMessageStrategy
    {
        void Send(Guid? candidateId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}