namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Communications;

    //todo: rename to less message type specific. maybe ISendUserMessageStrategy
    public interface ISendContactMessageStrategy
    {
        void Send(Guid? userId, MessageTypes messageType, IEnumerable<CommunicationToken> tokens);
    }
}
